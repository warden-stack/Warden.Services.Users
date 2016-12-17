using System.Threading.Tasks;
using Warden.Common.Events;
using Warden.Services.Features.Shared.Events;
using Warden.Services.Users.Repositories;

namespace Warden.Services.Users.Handlers
{
    public class UserPaymentPlanCreatedHandler : IEventHandler<UserPaymentPlanCreated>
    {
        private readonly IUserRepository _userRepository;

        public UserPaymentPlanCreatedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UserPaymentPlanCreated @event)
        {
            var maybeUser = await _userRepository.GetByUserIdAsync(@event.UserId);
            if (maybeUser.HasNoValue)
                return;
            if (maybeUser.Value.PaymentPlanId == @event.PlanId)
                return;

            maybeUser.Value.SetPaymentPlanId(@event.PlanId);
            await _userRepository.UpdateAsync(maybeUser.Value);
        }
    }
}