using Warden.Common.Commands;

namespace Warden.Services.Users.Shared.Commands
{
    public class SetNewPassword : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }
}