

using Core.Models;

namespace Core.AuthModels
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
