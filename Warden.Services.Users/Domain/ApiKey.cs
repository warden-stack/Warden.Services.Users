using System;
using Warden.Common.Domain;
using Warden.Common.Exceptions;
using Warden.Common.Extensions;
using Warden.Services.Users.Shared;

namespace Warden.Services.Users.Domain
{
    public class ApiKey : Entity, ITimestampable
    {
        public Guid Id { get; protected set; }
        public string Key { get; protected set; }
        public string UserId { get; protected set; }
        public string Name { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected ApiKey()
        {
        }

        public ApiKey(Guid id, string key, string userId, string name)
        {
            if (key.Empty())
            {
                throw new DomainException(OperationCodes.InvalidApiKey,
                    "API key can not be empty.");
            }
            if (userId.Empty())
            {
                throw new DomainException(OperationCodes.InvalidApiKey,
                    "Can not create an API key without user.");
            }
            if (name.Empty())
            {
                throw new DomainException(OperationCodes.InvalidApiKey,
                    "Can not create an API key without name.");
            }

            Id = id;
            Key = key;
            UserId = userId;
            Name = name.ToLowerInvariant();
            CreatedAt = DateTime.UtcNow;
        }
    }
}