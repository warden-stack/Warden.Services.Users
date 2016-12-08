﻿using System;
using Warden.Common.Commands;

namespace Warden.Services.Users.Shared.Commands
{
    public class SignIn : ICommand
    {
        public Request Request { get; set; }
        public Guid SessionId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string AccessToken { get; set; }
        public string Provider { get; set; }
    }
}