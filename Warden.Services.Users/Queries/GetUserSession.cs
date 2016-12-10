using System;
using Warden.Common.Queries;

namespace Warden.Services.Users.Queries
{
    public class GetUserSession : IQuery
    {
        public Guid Id { get; set; }
    }
}