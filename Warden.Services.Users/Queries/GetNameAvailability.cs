using Warden.Common.Queries;

namespace Warden.Services.Users.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}