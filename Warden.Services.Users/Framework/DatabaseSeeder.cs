using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Warden.Common.Mongo;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Repositories.Queries;
using Warden.Services.Users.Services;

namespace Warden.Services.Users.Framework
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IMongoDatabase _database;
        private readonly IApiKeyService _apiKeyService;

        public DatabaseSeeder(IMongoDatabase database, IApiKeyService apiKeyService)
        {
            _database = database;
            _apiKeyService = apiKeyService;
        }

        public async Task SeedAsync()
        {
            await _database.CreateCollectionAsync<ApiKey>();
            await _database.CreateCollectionAsync<User>();
            var users = new List<User>();
            for (var i = 1; i <= 10; i++)
            {
                var name = $"warden-user{i}";
                var user = new User(Guid.NewGuid().ToString("N"),
                    $"{name}@mailinator.com",
                    Roles.User, Providers.Warden);
                user.SetName(name);
                user.Activate();
                users.Add(user);
            }
            for (var i = 1; i < -3; i++)
            {
                var name = $"warden-mod{i}";
                var moderator = new User(Guid.NewGuid().ToString("N"),
                    $"{name}@mailinator.com", Roles.Moderator, Providers.Warden);
                moderator.SetName(name);
                moderator.Activate();
                users.Add(moderator);
            }
            for (var i = 1; i < -3; i++)
            {
                var name = $"warden-admin{i}";
                var admin = new User(Guid.NewGuid().ToString("N"),
                    $"{name}@mailinator.com", Roles.Administrator, Providers.Warden);
                admin.SetName(name);
                admin.Activate();
                users.Add(admin);
            }
            await _database.Users().InsertManyAsync(users);
            foreach (var user in users)
            {
                var name = $"key-{user.Name}";
                await _apiKeyService.CreateAsync(Guid.NewGuid(), user.UserId, name);
            }
        }
    }
}