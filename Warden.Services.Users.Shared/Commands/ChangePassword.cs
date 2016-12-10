using Warden.Common.Commands;

namespace Warden.Services.Users.Shared.Commands
{
    public class ChangePassword : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}