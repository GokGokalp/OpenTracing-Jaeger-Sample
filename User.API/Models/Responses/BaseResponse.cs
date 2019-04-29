using System.Collections.Generic;
using System.Linq;

namespace User.API.Models.Responses
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            Errors = new List<string>();
        }

        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public bool HasError { get { return Errors.Any();  } }
    }
}