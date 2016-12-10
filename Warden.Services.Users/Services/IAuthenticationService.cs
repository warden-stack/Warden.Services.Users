using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;

namespace Warden.Services.Users.Services
{
    public interface IAuthenticationService
    {
        Task<Maybe<UserSession>> GetSessionAsync(Guid id);

        Task SignInAsync(Guid sessionId, string email, string password,
            string ipAddress = null, string userAgent = null);

        Task SignInViaFacebookAsync(Guid sessionId, string accessToken,
            string ipAddress = null, string userAgent = null);

        Task SignOutAsync(Guid sessionId, string userId);

        Task CreateSessionAsync(Guid sessionId, string userId,
            string ipAddress = null, string userAgent = null);

        Task RefreshSessionAsync(Guid sessionId, string sessionKey,
            string ipAddress = null, string userAgent = null);
    }
}