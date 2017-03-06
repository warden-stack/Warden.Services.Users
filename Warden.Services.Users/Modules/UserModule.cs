using AutoMapper;
using Warden.Services.Users.Domain;
using Warden.Services.Users.Queries;
using Warden.Services.Users.Services;
using Warden.Services.Users.Dto;

namespace Warden.Services.Users.Modules
{
    public class UserModule : ModuleBase
    {
        public UserModule(IUserService userService, IMapper mapper) : base(mapper)
        {
            Get("users", async args => await FetchCollection<BrowseUsers, User>
                (async x => await userService.BrowseAsync(x))
                .MapTo<UserDto>()
                .HandleAsync());

            Get("users/{id}", async args => await Fetch<GetUser, User>
                (async x => await userService.GetAsync(x.Id))
                .MapTo<UserDto>()
                .HandleAsync());

           Get("usernames/{name}/available", async args => await Fetch<GetNameAvailability, dynamic>
               (async x => await userService.IsNameAvailableAsync(x.Name))
               .MapTo<AvailableResourceDto>()
               .HandleAsync());
        }
    }
}