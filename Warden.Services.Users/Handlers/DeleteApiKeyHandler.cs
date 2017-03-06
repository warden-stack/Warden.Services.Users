using System.Threading.Tasks;
using RawRabbit;
using Warden.Messages.Commands;
using Warden.Common.Handlers;
using Warden.Services.Users.Services;
using Warden.Messages.Commands.Users;
using Warden.Messages.Events.Users;

namespace Warden.Services.Users.Handlers
{
    public class DeleteApiKeyHandler : ICommandHandler<DeleteApiKey>
    {
        private readonly IHandler _handler;
        private readonly IBusClient _bus;
        private readonly IApiKeyService _apiKeyService;

        public DeleteApiKeyHandler(IHandler handler, IBusClient bus,
            IApiKeyService apiKeyService)
        {
            _bus = bus;
            _handler = handler;
            _apiKeyService = apiKeyService;
        }

        public async Task HandleAsync(DeleteApiKey command)
        {
            await _handler.Run(async () =>
                await _apiKeyService.DeleteAsync(command.UserId, command.Name))
            .OnSuccess(async () => 
                await _bus.PublishAsync(new ApiKeyDeleted(command.Request.Id, command.UserId, command.Name)))
            .OnCustomError(async ex => await _bus.PublishAsync(new DeleteApiKeyRejected(command.Request.Id,
                    command.Name, command.UserId, ex.Code, ex.Message)))
            .OnError(async (ex, logger) =>
            {
                logger.Error(ex, $"Error occured while deleting API key with name: '{command.Name}'.");
                await _bus.PublishAsync(new DeleteApiKeyRejected(command.Request.Id,
                    command.Name, command.UserId, OperationCodes.Error, ex.Message));
            })
            .ExecuteAsync();
        }        
    }
}