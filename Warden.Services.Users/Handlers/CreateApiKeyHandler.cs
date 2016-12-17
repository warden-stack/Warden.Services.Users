using System;
using System.Threading.Tasks;
using NLog;
using RawRabbit;
using Warden.Common.Commands;
using Warden.Services.Users.Services;
using Warden.Services.Users.Shared.Commands;
using Warden.Services.Users.Shared.Events;

namespace Warden.Services.Users.Handlers
{
    public class CreateApiKeyHandler : ICommandHandler<CreateApiKey>
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IBusClient _bus;

        public CreateApiKeyHandler(IApiKeyService apiKeyService,
            IBusClient bus)
        {
            _apiKeyService = apiKeyService;
            _bus = bus;
        }

        public async Task HandleAsync(CreateApiKey command)
        {
            await _apiKeyService.CreateAsync(Guid.NewGuid(), command.UserId, command.Name);
            await _bus.PublishAsync(new ApiKeyCreated(command.Request.Id, command.UserId, command.Name));
        }
    }
}