﻿using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;

namespace Warden.Services.Users.Services
{
    public interface IApiKeyService
    {
        Task<Maybe<PagedResult<ApiKey>>> BrowseAsync(BrowseApiKeys query);
        Task<Maybe<ApiKey>> GetAsync(string userId, string name);
        Task<Maybe<ApiKey>> GetAsync(Guid id);
        Task CreateAsync(Guid id, string userId, string name);
        Task DeleteAsync(string userId, string name);
    }
}