using FashionSystem.Web.Models.User;

namespace FashionSystem.Web.Services
{
    public class UserService
    {
        private readonly IHttpClientFactory _factory;

        public UserService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> Register(CreateUserDto dto)
        {
            var client = _factory.CreateClient("Api");

            var response =
                await client.PostAsJsonAsync(
                    "api/users",
                    dto);

            return response.IsSuccessStatusCode;
        }
    }
}
