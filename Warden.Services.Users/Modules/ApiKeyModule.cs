﻿using AutoMapper;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;
using Warden.Services.Users.Services;
using Warden.Services.Users.Dto;

namespace Warden.Services.Users.Modules
{
    public class ApiKeyModule : ModuleBase
    {
        public ApiKeyModule(IMapper mapper, IApiKeyService apiKeyService) : base(mapper)
        {
            Get("api-keys", async args => await FetchCollection<BrowseApiKeys, ApiKey>
                (async x => await apiKeyService.BrowseAsync(x))
                .MapTo<ApiKeyDto>()
                .HandleAsync());  

            Get("users/{userId}/api-keys", async args => await FetchCollection<BrowseApiKeys, ApiKey>
                (async x => await apiKeyService.BrowseAsync(x))
                .MapTo<ApiKeyDto>()
                .HandleAsync());            

            Get("users/{userId}/api-keys/{name}", async args => await Fetch<GetApiKey, ApiKey>
                (async x => await apiKeyService.GetAsync(x.UserId, x.Name))
                .MapTo<ApiKeyDto>()
                .HandleAsync());
        }
    }
}