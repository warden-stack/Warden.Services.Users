using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Repositories.Queries;

namespace Warden.Services.Users.Repositories
{
    public class OneTimeSecuredOperationRepository : IOneTimeSecuredOperationRepository
    {
        private readonly IMongoDatabase _database;

        public OneTimeSecuredOperationRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Maybe<OneTimeSecuredOperation>> GetAsync(Guid id)
            => await _database.OneTimeSecuredOperations().GetAsync(id);

        public async Task<Maybe<OneTimeSecuredOperation>> GetAsync(string type, string user, string token)
            => await _database.OneTimeSecuredOperations().GetAsync(type, user, token);

        public async Task AddAsync(OneTimeSecuredOperation operation)
            => await _database.OneTimeSecuredOperations().InsertOneAsync(operation);

        public async Task UpdateAsync(OneTimeSecuredOperation operation)
            => await _database.OneTimeSecuredOperations().ReplaceOneAsync(x => x.Id == operation.Id, operation);
    }
}