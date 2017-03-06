﻿using System;

namespace Warden.Services.Users.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public string ExternalUserId { get; set; }
        public Guid? PaymentPlanId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}