using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class PasswordChanged : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get;}

        protected PasswordChanged()
        {
        }

        public PasswordChanged(Guid requestId, string userId)
        {
            RequestId = requestId;
            UserId = userId;
        }
    }
}