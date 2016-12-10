using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class UserNameChanged : IAuthenticatedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string NewName { get; }
        public string State { get; }

        protected UserNameChanged()
        {
        }

        public UserNameChanged(Guid requestId, string userId, string newName, string state)
        {
            RequestId = requestId;
            UserId = userId;
            NewName = newName;
            State = state;
        }
    }
}