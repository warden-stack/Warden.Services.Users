using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;

namespace Warden.Services.Users.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string name);
        Task<Maybe<User>> GetByUserIdAsync(string userId);
        Task<Maybe<User>> GetByExternalUserIdAsync(string externalUserId);
        Task<Maybe<User>> GetByEmailAsync(string email, string provider);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}