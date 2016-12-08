using System;
using Warden.Common.Commands;

namespace Warden.Services.Users.Shared.Commands
{
    public class SignOut : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public Guid SessionId { get; set; }
        public string UserId { get; set; }
    }
}