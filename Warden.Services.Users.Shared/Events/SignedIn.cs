using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class SignedIn : IEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Email { get; }
        public string Provider { get; set; }

        protected SignedIn()
        {
        }

        public SignedIn(Guid requestId, string userId, string email, string provider)
        {
            RequestId = requestId;
            UserId = userId;
            Email = email;
            Provider = provider;
        }
    }
}