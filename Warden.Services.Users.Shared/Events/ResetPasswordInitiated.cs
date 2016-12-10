using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class ResetPasswordInitiated : IEvent
    {
        public Guid RequestId { get; }
        public string Email { get; }

        protected ResetPasswordInitiated()
        {
        }

        public ResetPasswordInitiated(Guid requestId, string email)
        {
            RequestId = requestId;
            Email = email;
        }
    }
}