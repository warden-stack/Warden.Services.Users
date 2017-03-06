using System.Threading.Tasks;
using RawRabbit;
using Warden.Messages.Commands;
using Warden.Common.Handlers;
using Warden.Services.Users.Services;
using Warden.Messages.Commands.Users;
using Warden.Messages.Events.Users;

namespace Warden.Services.Users.Handlers
{
    public class ChangeUsernameHandler : ICommandHandler<ChangeUsername>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IUserService _userService;

        public ChangeUsernameHandler(IHandler handler, 
            IBusClient bus, IUserService userService)
        {
            _handler = handler;
            _bus = bus;
            _userService = userService;
        }

        public async Task HandleAsync(ChangeUsername command)
        {
            await _handler
                .Run(async () => await _userService.ChangeNameAsync(command.UserId, command.Name))
                .OnSuccess(async () =>
                {
                    var user = await _userService.GetAsync(command.UserId);
                    await _bus.PublishAsync(new UsernameChanged(command.Request.Id, 
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