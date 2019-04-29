using System.Collections.Generic;

namespace User.Common.Contracts
{
    public class UserRegisteredEvent
    {
        public string Email { get; set; }
        public Dictionary<string, string> TracingKeys { get; set; }
    }
}