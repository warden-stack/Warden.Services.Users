using AutoMapper;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;
using Warden.Services.Users.Services;
using Warden.Services.Users.Dto;

namespace Warden.Services.Users.Modules
{
    public class UserSessionModule : ModuleBase
    {
        public UserSessionModule(IAuthenticationService authenticationService, IMapper mapper)
            : base(mapper, "user-sessions")
        {
            Get("{id}", async args => await Fetch<GetUserSession, UserSession>
                (async x => await authenticationService.GetSessionAsync(x.Id))
                .MapTo<UserSessionDto>()
                .HandleAsync());
        }
    }
}