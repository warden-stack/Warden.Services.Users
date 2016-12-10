﻿using System.Threading.Tasks;
using RawRabbit;
using Warden.Common.Commands;
using Warden.Common.Handlers;
using Warden.Services.Users.Services;
using Warden.Services.Users.Shared;
using Warden.Services.Users.Shared.Commands;
using Warden.Services.Users.Shared.Events;

namespace Warden.Services.Users.Handlers
{
    public class ChangeUserNameHandler : ICommandHandler<ChangeUserName>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IUserService _userService;

        public ChangeUserNameHandler(IHandler handler, 
            IBusClient bus, IUserService userService)
        {
            _handler = handler;
            _bus = bus;
            _userService = userService;
        }

        public async Task HandleAsync(ChangeUserName command)
        {
            await _handler
                .Run(async () => await _userService.ChangeNameAsync(command.UserId, command.Name))
                .OnSuccess(async () =>
                {
                    var user = await _userService.GetAsync(command.UserId);
                    await _bus.PublishAsync(new UserNameChanged(command.Request.Id, 
                        command.UserId, command.Name, user.Value.State));
                })
                .OnCustomError(async ex => await _bus.PublishAsync(new ChangeUsernameRejected(command.Request.Id,
                    command.UserId, ex.Code, ex.Message, command.Name)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, "Error occured while changing username");
                    await _bus.PublishAsync(new ChangeUsernameRejected(command.Request.Id,
                        command.UserId, OperationCodes.Error, ex.Message, command.Name));
                })
                .ExecuteAsync();
        }
    }
}