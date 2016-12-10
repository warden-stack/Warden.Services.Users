using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;

namespace Warden.Services.Users.Services
{
    public interface IUserService
    {
        Task<bool> IsNameAvailableAsync(string name);
        Task<Maybe<User>> GetAsync(string userId);
        Task<Maybe<User>> GetByNameAsync(string name);
        Task<Maybe<User>> GetByExternalUserIdAsync(string externalUserId);
        Task<Maybe<User>> GetByEmailAsync(string email, string provider);
        Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query);

        Task SignUpAsync(string userId, string email, string role,
            string provider, string password = null,
            string externalUserId = null,
            bool activate = true, string pictureUrl = null,
            string name = null);

        Task ChangeNameAsync(string userId, string name);
    }
}