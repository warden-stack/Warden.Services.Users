﻿using System;
using System.Threading.Tasks;
using NLog;
using RawRabbit;
using Warden.Common.Commands;
using Warden.Common.Handlers;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Services;
using Warden.Services.Users.Shared;
using Warden.Services.Users.Shared.Commands;
using Warden.Services.Users.Shared.Events;

namespace Warden.Services.Users.Handlers
{
    public class SignInHandler : ICommandHandler<SignIn>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IUserService _userService;
        private readonly IFacebookService _facebookService;
        private readonly IAuthenticationService _authenticationService;

        public SignInHandler(IHandler handler,
            IBusClient bus,
            IUserService userService,
            IFacebookService facebookService,
            IAuthenticationService authenticationService)
        {
            _handler = handler;
            _bus = bus;
            _userService = userService;
            _facebookService = facebookService;
            _authenticationService = authenticationService;
        }

        public async Task HandleAsync(SignIn command)
        {
            Maybe<User> user = null;
            await _handler
                .Run(async () =>
                {
                    switch (command.Provider?.ToLowerInvariant())
                    {
                        case "warden":
                            user = await HandleDefaultSignInAsync(command);
                            break;
                        case "facebook":
                            user = await HandleFacebookSignInAsync(command);
                            break;
                        default:
                            throw new ArgumentException($"Invalid provider: {command.Provider}", nameof(command.Provider));
                    }
                })
                .OnSuccess(async () => await _bus.PublishAsync(new SignedIn(command.Request.Id,
                    user.Value.UserId, user.Value.Email, user.Value.Name, user.Value.Provider)))
                .OnCustomError(async ex => await _bus.PublishAsync(new SignInRejected(command.Request.Id,
                    null, ex.Code, ex.Message, command.Provider)))
                .OnError(async (ex, logger) =>
                {
                    Logger.Error(ex, "Error occured while signing in");
                    await _bus.PublishAsync(new SignInRejected(command.Request.Id,
                        null, OperationCodes.Error, ex.Message, command.Provider));
                })
                .ExecuteAsync();
        }

        private async Task<Maybe<User>> HandleDefaultSignInAsync(SignIn command)
        {
            await _authenticationService.SignInAsync(command.SessionId,
                command.Email, command.Password, command.IpAddress, command.UserAgent);

            return await _userService.GetByEmailAsync(command.Email, command.Provider);
        }

        private async Task<Maybe<User>> HandleFacebookSignInAsync(SignIn command)
        {
            var facebookUser = await _facebookService.GetUserAsync(command.AccessToken);
            if (facebookUser.HasNoValue)
                return new Maybe<User>();

            var externalUserId = facebookUser.Value.Id;
            var user = await _userService.GetByExternalUserIdAsync(externalUserId);
            if (user.HasValue)
            {
                await _authenticationService.SignInViaFacebookAsync(command.SessionId,
                    command.AccessToken, command.IpAddress, command.UserAgent);

                return user;
            }

            var userId = Guid.NewGuid().ToString("N");
            await _userService.SignUpAsync(userId, facebookUser.Value.Email,
                Roles.User, Providers.Facebook, externalUserId: externalUserId);

            Logger.Info($"Created new user with id: '{userId}' using Facebook user id: '{externalUserId}'");

            user = await _userService.GetByExternalUserIdAsync(externalUserId);
            await _bus.PublishAsync(new SignedUp(command.Request.Id, userId, user.Value.Email,
                user.Value.Name, string.Empty, user.Value.Role, user.Value.State,
                user.Value.Provider, user.Value.ExternalUserId, user.Value.CreatedAt));

            await _authenticationService.SignInViaFacebookAsync(command.SessionId, command.AccessToken,
                command.IpAddress, command.UserAgent);

            return await _userService.GetByExternalUserIdAsync(externalUserId);
        }
    }
}