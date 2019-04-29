using System;
using System.Threading.Tasks;
using User.API.Models.Requests;
using User.API.Models.Responses;
using MassTransit;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using OpenTracing.Propagation;
using System.Collections.Generic;
using User.Common.Contracts;

namespace User.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IBusControl _busControl;
        private readonly ITracer _tracer;

        public UserService(ILogger<UserService> logger, IBusControl busControl, ITracer tracer)
        {
            _logger = logger;
            _busControl = busControl;
            _tracer = tracer;
        }

        public async Task<BaseResponse<int>> CreateUserAsync(CreateUserRequest request)
        {
            BaseResponse<int> createUserResponse = new BaseResponse<int>();

            try
            {
                using (var scope = _tracer.BuildSpan("create-user-async").StartActive(finishSpanOnDispose: true))
                {
                    var span = scope.Span.SetTag(Tags.SpanKind, Tags.SpanKindClient);

                    var dictionary = new Dictionary<string, string>();
                    _tracer.Inject(span.Context, BuiltinFormats.TextMap, new TextMapInjectAdapter(dictionary));

                    //some user create business logics

                    createUserResponse.Data = 1; // User id

                    await _busControl.Publish(new UserRegisteredEvent
                    {
                        Email = request.Email,
                        TracingKeys = dictionary
                    });
                }
            }
            catch (Exception ex)
            {
                createUserResponse.Errors.Add(ex.Message);
                _logger.LogError(ex, ex.Message);
            }

            return createUserResponse;
        }
    }
}