﻿using AutoMapper;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Dto;

namespace Warden.Services.Users.Framework
{
    public class AutoMapperConfig
    {
        public static IMapper InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApiKey, ApiKeyDto>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<dynamic, AvailableResourceDto>()
                   .ForMember(x => x.IsAvailable, o => o.MapFrom(s => s));
                cfg.CreateMap<UserSession, UserSessionDto>();
            });

            return config.CreateMapper();
        }
    }
}