using System;

namespace Warden.Services.Users.Dto
{
    public class ApiKeyDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}