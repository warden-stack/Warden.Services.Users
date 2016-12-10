using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;

namespace Warden.Services.Users.Repositories
{
    public interface IUserSessionRepository
    {
        Task<Maybe<UserSession>> GetByIdAsync(Guid id);
        Task AddAsync(UserSession session);
        Task UpdateAsync(UserSession session);
    }
}