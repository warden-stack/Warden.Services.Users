using System;
using System.Threading.Tasks;
using Warden.Common.Exceptions;
using Warden.Common.Extensions;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;
using Warden.Services.Users.Repositories;
using Warden.Services.Users;

namespace Warden.Services.Users.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly int RetryTimes = 5;
        private readonly IApiKeyRepository _repository;
        private readonly IEncrypter _encrypter;

        public ApiKeyService(IApiKeyRepository repository,
            IEncrypter encrypter)
        {
            _repository = repository;
            _encrypter = encrypter;
        }

        public async Task<Maybe<PagedResult<ApiKey>>> BrowseAsync(BrowseApiKeys query)
            => await _repository.BrowseAsync(query);

        public async Task<Maybe<ApiKey>> GetAsync(string userId, string name)
            => await _repository.GetAsync(userId, name);

        public async Task<Maybe<ApiKey>> GetAsync(Guid id) 
            => await _repository.GetAsync(id);

        public async Task CreateAsync(Guid id, string userId, string name)
        {
            if (name.Empty())
            {
                 throw new ServiceException(OperationCodes.InvalidApiKey,
                    $"API key with must have a name.");               
            }

            var existingApiKey = await _repository.GetAsync(userId, name);
            if (existingApiKey.HasValue) 
            {
                throw new ServiceException(OperationCodes.ApiKeyNameInUse,
                    $"API key with name: '${name}' already exists.");
            }
            
            var isValid = false;
            var currentTry = 0;
            var key = string.Empty;
            while (currentTry < RetryTimes)
            {
                key = _encrypter.GetRandomSecureKey();
                isValid = (await _repository.GetAsync(key)).HasNoValue;
                if (isValid)
                    break;

                currentTry++;
            }

            if (!isValid)
            {
                throw new ServiceException(OperationCodes.Error,
                    "Could not create an API key, please try again.");
            }

            var apiKey = new ApiKey(id, key, userId, name);
            await _repository.AddAsync(apiKey);
        }

        public async Task DeleteAsync(string userId, string name)
        {
            var apiKey = await _repository.GetAsync(userId, name);
            if (apiKey.HasNoValue)
            {
                throw new ServiceException(OperationCodes.ApiKeyNotFound, 
                    $"Desired API key with name: '{name}' was not found!.");
            }

            await _repository.DeleteAsync(apiKey.Value.Id);
        }
    }
}