using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;

namespace Warden.Services.Users.Repositories
{
    public interface IApiKeyRepository
    {
        Task<Maybe<PagedResult<ApiKey>>> BrowseAsync(BrowseApiKeys query);
        Task<Maybe<ApiKey>> GetAsync(Guid id);
        Task<Maybe<ApiKey>> GetAsync(string key);
        Task<Maybe<ApiKey>> GetAsync(string userId, string name);
        Task AddAsync(ApiKey apiKey);
        Task DeleteAsync(string key);
    }
}