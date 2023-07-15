using admission_task.Common;
using admission_task.Dtos;

namespace admission_task.Repos.Interfaces
{
    public interface IAuthRepository
    {
        Task<Response> RegisterAsync(RegisterDto user);
        Task<LoginResponse> LoginAsync(LoginDto request);


    }
}
