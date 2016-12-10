﻿using System;
using Warden.Common.Domain;
using Warden.Common.Exceptions;
using Warden.Common.Extensions;
using Warden.Services.Users.Shared;

namespace Warden.Services.Users.Domain
{
    public class OneTimeSecuredOperation : IdentifiableEntity, ITimestampable
    {
        public string Type { get; protected set; }
        public string User { get; protected set; }
        public string Token { get; protected set; }
        public string RequesterIpAddress { get; protected set; }
        public string RequesterUserAgent { get; protected set; }
        public string ConsumerIpAddress { get; protected set; }
        public string ConsumerUserAgent { get; protected set; }
        public bool Consumed => ConsumedAt.HasValue;
        public DateTime CreatedAt { get; protected set; }
        public DateTime? ConsumedAt { get; protected set; }
        public DateTime Expiry { get; protected set; }

        protected OneTimeSecuredOperation()
        {
        }

        public OneTimeSecuredOperation(Guid id, string type,
            string user, string token, DateTime expiry,
            string ipAddress = null, string userAgent = null)
        {
            if (type.Empty())
            {
                throw new DomainException(OperationCodes.InvalidSecuredOperation,
                    "Type can not be empty.");
            }
            if (user.Empty())
            {
                throw new DomainException(OperationCodes.InvalidSecuredOperation,
                    "User can not be empty.");
            }
            if (token.Empty())
            {
                throw new DomainException(OperationCodes.InvalidSecuredOperation,
                    "Token can not be empty.");
            }
            Id = id;
            Type = type;
            User = user;
            Token = token;
            Expiry = expiry.ToUniversalTime();
            RequesterIpAddress = ipAddress;
            RequesterUserAgent = userAgent;
            CreatedAt = DateTime.UtcNow;
        }

        public void Consume(string ipAddress = null, string userAgent = null)
        {
            if (!CanBeConsumed())
            {
                throw new DomainException(OperationCodes.InvalidSecuredOperation,
                    "Operation can not be consumed.");
            }

            ConsumerIpAddress = ipAddress;
            ConsumerUserAgent = userAgent;
            ConsumedAt = DateTime.UtcNow;
        }

        public bool CanBeConsumed()
        {
            if (Consumed)
                return false;

            return Expiry > DateTime.UtcNow;
        }
    }
}