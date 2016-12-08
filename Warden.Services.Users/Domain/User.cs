﻿using System;
using Warden.Common.Domain;
using Warden.Common.Exceptions;
using Warden.Common.Extensions;

namespace Warden.Services.Users.Domain
{
    public class User : IdentifiableEntity, ITimestampable
    {
        public string UserId { get; set; }
        public string Email { get; protected set; }
        public string Role { get; protected set; }
        public string State { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }
        public Guid? PaymentPlanId { get; protected set; }

        protected User()
        {
        }

        public User(string userId, string email, string role)
        {
            SetUserId(userId);
            SetEmail(email);
            Role = role;
            State = States.Inactive;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetEmail(string email)
        {
            if (email.Empty())
            {
                Email = string.Empty;
                UpdatedAt = DateTime.UtcNow;

                return;

            }
            if (!email.IsEmail())
                throw new DomainException($"Invalid email: '{email}.");

            if (Email.EqualsCaseInvariant(email))
                return;

            Email = email.ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetUserId(string userId)
        {
            if (userId.Empty())
                throw new DomainException("User id cannot be empty");

            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetRole(string role)
        {
            if (Role == role)
                return;

            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }


        public void Lock()
        {
            if(State == States.Locked)
                return;

            State = States.Locked;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (State == States.Active)
                return;

            State = States.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            if (State == States.Deleted)
                return;

            State = States.Deleted;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPaymentPlanId(Guid? planId)
        {
            PaymentPlanId = planId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}