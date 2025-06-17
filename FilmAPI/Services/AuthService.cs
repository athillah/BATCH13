using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmAPI.Models;

namespace FilmAPI.Services
{
    public interface IAuthService
    {
        Task<string> GenerateJwtTokenAsync(User user, IList<string> roles);
    }

    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }
            

        public async Task<string> GenerateJwtTokenAsync(User user, IList<string> roles)
        {
            return await _jwtTokenService.GenerateTokenAsync(user, roles);
        }
    }
}