using System.Threading.Tasks;
using MassTransit;
using OpenTracing;
using User.Activation.Consumer.Common;
using User.Common.Contracts;

namespace User.Activation.Consumer.Consumers
{
    public class UserActivationConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly ITracer _tracer;

        public UserActivationConsumer(ITracer tracer)
        {
            _tracer = tracer;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            using (var scope = TracingExtension.StartServerSpan(_tracer, context.Message.TracingKeys, "user-activation-link-sender-consumer"))
            {
                //some user activation link send business logics

                await System.Console.Out.WriteLineAsync($"Activation link sent for {context.Message.Email}");
            }
        }
    }
}