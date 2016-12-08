using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class SignInRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Code { get; }
        public string Reason { get; }
        public string Provider { get; }

        protected SignInRejected()
        {
        }

        public SignInRejected(Guid requestId,
            string userId, string code,
            string reason, string provider)
        {
            RequestId = requestId;
            UserId = userId;
            Code = code;
            Reason = reason;
            Provider = provider;
        }
    }
}