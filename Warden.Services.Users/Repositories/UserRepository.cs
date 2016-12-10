using System.Threading.Tasks;
using MongoDB.Driver;
using Warden.Common.Types;
using Warden.Common.Mongo;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;
using Warden.Services.Users.Repositories.Queries;

namespace Warden.Services.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string name)
            => await _database.Users().ExistsAsync(name);

        public async Task<Maybe<User>> GetByUserIdAsync(string userId)
            => await _database.Users().GetByUserIdAsync(userId);

        public async Task<Maybe<User>> GetByExternalUserIdAsync(string externalUserId)
            => await _database.Users().GetByExternalUserIdAsync(externalUserId);

        public async Task<Maybe<User>> GetByEmailAsync(string email, string provider)
            => await _database.Users().GetByEmailAsync(email, provider);

        public async Task<Maybe<User>> GetByNameAsync(string name)
            => await _database.Users().GetByNameAsync(name);

        public async Task<Maybe<PagedResult<User>>> BrowseAsync(BrowseUsers query)
        {
            return await _database.Users()
                .Query(query)
                .PaginateAsync(query);
        }

        public async Task AddAsync(User user)
            => await _database.Users().InsertOneAsync(user);

        public async Task UpdateAsync(User user)
            => await _database.Users().ReplaceOneAsync(x => x.Id == user.Id, user);
    }
}