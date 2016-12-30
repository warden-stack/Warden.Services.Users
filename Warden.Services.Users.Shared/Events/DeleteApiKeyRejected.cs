using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class DeleteApiKeyRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string Name { get; }
        public string UserId { get; }
        public string Code { get; }
        public string Reason { get; }


        protected DeleteApiKeyRejected()
        {
        }

        public DeleteApiKeyRejected(Guid requestId, string name,
            string userId, string code, string reason)
        {
            RequestId = requestId;
            Name = name;
            UserId = userId;
            Code = code;
            Reason = reason;
        }       
    }
}