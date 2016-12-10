using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Repositories.Queries;

namespace Warden.Services.Users.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IMongoDatabase _database;

        public UserSessionRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<UserSession>> GetByIdAsync(Guid id)
            => await _database.UserSessions().GetByIdAsync(id);

        public async Task AddAsync(UserSession session)
            => await _database.UserSessions().InsertOneAsync(session);

        public async Task UpdateAsync(UserSession session)
            => await _database.UserSessions().ReplaceOneAsync(x => x.Id == session.Id, session);
    }
}