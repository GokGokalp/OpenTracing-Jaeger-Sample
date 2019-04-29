using System.Threading.Tasks;
using User.API.Models.Requests;
using User.API.Models.Responses;

namespace User.API.Services
{
    public interface IUserService
    {
         Task<BaseResponse<int>> CreateUserAsync(CreateUserRequest request);
    }
}