using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Users.Domain;

namespace Warden.Services.Users.Services
{
    public interface IFacebookService
    {
        Task<Maybe<FacebookUser>> GetUserAsync(string accessToken);
        Task<bool> ValidateTokenAsync(string accessToken);
    }
}