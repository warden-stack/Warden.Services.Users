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
    public class SetNewPasswordHandler : ICommandHandler<SetNewPassword>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IPasswordService _passwordService;

        public SetNewPasswordHandler(IHandler handler, 
            IBusClient bus, 
            IPasswordService passwordService)
        {
            _handler = handler;
            _bus = bus;
            _passwordService = passwordService;
        }

        public async Task HandleAsync(SetNewPassword command)
        {
            await _handler
                .Run(async () => await _passwordService.SetNewAsync(command.Email, command.Token, command.Password))
                .OnSuccess(async () => await _bus.PublishAsync(new NewPasswordSet(command.Request.Id, command.Email)))
                .OnCustomError(async ex => await _bus.PublishAsync(new SetNewPasswordRejected(command.Request.Id,
                    ex.Code, ex.Message, command.Email)))
                .OnError(async (ex, logger) =>
                {
                    logger.Error(ex, "Error when trying to set new password.");
                    await _bus.PublishAsync(new SetNewPasswordRejected(command.Request.Id,
                        OperationCodes.Error, "Error when trying to set new password.", command.Email));
                })
                .ExecuteAsync();
        }
    }
}