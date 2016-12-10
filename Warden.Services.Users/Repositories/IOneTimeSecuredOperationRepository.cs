using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;

namespace Warden.Services.Users.Repositories
{
    public interface IOneTimeSecuredOperationRepository
    {
        Task<Maybe<OneTimeSecuredOperation>> GetAsync(Guid id);
        Task<Maybe<OneTimeSecuredOperation>> GetAsync(string type, string user, string token);
        Task AddAsync(OneTimeSecuredOperation operation);
        Task UpdateAsync(OneTimeSecuredOperation operation);
    }
}