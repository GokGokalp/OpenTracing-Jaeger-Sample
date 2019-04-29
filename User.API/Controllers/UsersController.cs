using Microsoft.AspNetCore.Mvc;
using User.API.Services;
using User.API.Models.Requests;
using User.API.Models.Responses;
using System.Threading.Tasks;

namespace User.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateUserRequest request)
        {
            BaseResponse<int> createUserResponse = await _userService.CreateUserAsync(request);

            if(!createUserResponse.HasError)
            {
                return Created("users", createUserResponse.Data);
            }
            else
            {
                return BadRequest(createUserResponse.Errors);
            }
        }
    }
}