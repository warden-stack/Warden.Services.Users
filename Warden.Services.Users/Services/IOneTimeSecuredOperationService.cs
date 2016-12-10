using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;

namespace Warden.Services.Users.Services
{
    public interface IOneTimeSecuredOperationService
    {
        Task<Maybe<OneTimeSecuredOperation>> GetAsync(Guid id);
        Task CreateAsync(Guid id, string type, string user, DateTime expiry);
        Task<bool> CanBeConsumedAsync(string type, string user, string token);
        Task ConsumeAsync(string type, string user, string token);
    }
}