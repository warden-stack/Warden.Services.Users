using Warden.Common.Queries;

namespace Warden.Services.Users.Queries
{
    public class GetApiKey : IQuery
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}