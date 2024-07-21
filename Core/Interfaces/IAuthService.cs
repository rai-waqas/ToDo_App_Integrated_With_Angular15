using Core.AuthModels;
namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(LoginRequest request);
    }
}
