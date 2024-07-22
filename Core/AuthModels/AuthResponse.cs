

using Core.Models;

namespace Core.AuthModels
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public int userId { get; set; } = -1;
    }
}
