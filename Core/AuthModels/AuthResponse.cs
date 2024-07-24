
using Core.AuthDto;

namespace Core.AuthModels
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public AuthResponseDto User { get; set; } = null!;
    }
}
