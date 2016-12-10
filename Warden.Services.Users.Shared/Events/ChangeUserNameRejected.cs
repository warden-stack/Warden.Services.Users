﻿using System;
using Warden.Common.Events;

namespace Warden.Services.Users.Shared.Events
{
    public class ChangeUsernameRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Code { get; }
        public string RejectedUsername { get; }

        protected ChangeUsernameRejected(string rejectedUsername)
        {
            RejectedUsername = rejectedUsername;
        }

        public ChangeUsernameRejected(Guid requestId,
            string userId, string code,
            string reason, string rejectedUsername)
        {
            RequestId = requestId;
            UserId = userId;
            Code = code;
            Reason = reason;
            RejectedUsername = rejectedUsername;
        }
    }
}