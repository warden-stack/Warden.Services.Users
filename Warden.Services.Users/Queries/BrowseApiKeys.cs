using Warden.Common.Queries;

namespace Warden.Services.Users.Queries
{
    public class BrowseApiKeys : PagedQueryBase
    {
        public string UserId { get; set; }
    }
}