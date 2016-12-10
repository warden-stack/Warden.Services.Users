using Warden.Common.Commands;

namespace Warden.Services.Users.Shared.Commands
{
    public class ResetPassword : ICommand
    {
        public Request Request { get; set; }
        public string Email { get; set; }
        public string Endpoint { get; set; }
    }
}