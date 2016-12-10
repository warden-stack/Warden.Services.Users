using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class NewPasswordSet : IEvent
    {
        public Guid RequestId { get; }
        public string Email { get; }

        protected NewPasswordSet()
        {
        }

        public NewPasswordSet(Guid requestId, string email)
        {
            RequestId = requestId;
            Email = email;
        }
    }
}