using FashionSystem.Web.Models;
using FashionSystem.Web.Models.Auth;
using System.Net.Http.Json;

namespace FashionSystem.Web.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _factory;

        public AuthService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<string?> Login(LoginDto dto)
        {
            var client = _factory.CreateClient("Api");

            var response = await client.PostAsJsonAsync(
                "api/auth/login",
                dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result =
                await response.Content
                    .ReadFromJsonAsync<TokenResponse>();

            return result?.Token;
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}