using Warden.Common.Host;
using Warden.Services.Users.Framework;
using Warden.Messages.Commands.Users;
using Warden.Messages.Events.Features;

namespace Warden.Services.Users
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 5051)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<SignIn>()
                .SubscribeToCommand<SignUp>()
                .SubscribeToCommand<SignOut>()
                .SubscribeToCommand<ChangeUsername>()
                .SubscribeToCommand<ChangePassword>()
                .SubscribeToCommand<ResetPassword>()
                .SubscribeToCommand<SetNewPassword>()
                .SubscribeToCommand<CreateApiKey>()
                .SubscribeToCommand<DeleteApiKey>()                
                .SubscribeToEvent<UserPaymentPlanCreated>()
                .Build()
                .Run();
        }
    }
}
