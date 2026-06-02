using FashionSystem.Web.Models.Rental;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FashionSystem.Web.Services
{
    public class RentalService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _contextAccessor;

        public RentalService(
            IHttpClientFactory factory,
            IHttpContextAccessor contextAccessor)
        {
            _factory = factory;
            _contextAccessor = contextAccessor;
        }

        private HttpClient CreateClient()
        {
            var client = _factory.CreateClient("Api");

            var token = _contextAccessor
                .HttpContext?
                .Session
                .GetString("JWT");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            return client;
        }

        // CREATE RENTAL
        public async Task<(bool Success, string Message)>
            Create(CreateRentalDto dto)
        {
            var client = CreateClient();

            var response =
                await client.PostAsJsonAsync(
                    "api/Rentals",
                    dto);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Rental created successfully.");
            }

            var error =
                await response.Content.ReadAsStringAsync();

            return (false, error);
        }

        // GET USER RENTALS
        public async Task<List<GetRentalDto>>
            GetUserRentals(int userId)
        {
            var client = CreateClient();

            var rentals =
                await client.GetFromJsonAsync<
                    List<GetRentalDto>>(
                    $"api/Rentals/user/{userId}");

            return rentals ?? new List<GetRentalDto>();
        }

        // RETURN RENTAL
        public async Task<bool> ReturnRental(int id)
        {
            var client = CreateClient();

            var response =
                await client.PutAsync(
                    $"api/rentals/{id}/return",
                    null);

            return response.IsSuccessStatusCode;
        }
    }
}