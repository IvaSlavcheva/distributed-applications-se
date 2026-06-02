using FashionSystem.Web.Models.FashionItem;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FashionSystem.Web.Models;

namespace FashionSystem.Web.Services
{
    public class FashionItemService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _contextAccessor;

        public FashionItemService(
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

        // GET ALL
        public async Task<PagedResult<GetFashionItemDto>> GetAll(
            string? designer = null,
            string? category = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 8)
        {
            var client = CreateClient();

            var url =
                $"api/FashionItems?page={page}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(designer))
                url += $"&designer={designer}";

            if (!string.IsNullOrEmpty(category))
                url += $"&category={category}";

            if (!string.IsNullOrEmpty(sortBy))
                url += $"&sortBy={sortBy}";

            var result =
                await client.GetFromJsonAsync<PagedResult<GetFashionItemDto>>(url);

            return result ?? new PagedResult<GetFashionItemDto>();
        }
        // GET BY ID
        public async Task<GetFashionItemDto?> GetById(int id)
        {
            var client = CreateClient();

            return await client
                .GetFromJsonAsync<GetFashionItemDto>(
                    $"api/FashionItems/{id}");
        }
        // CREATE
        public async Task<bool> Create(
            CreateFashionItemDto dto)
        {
            var client = CreateClient();

            var form = new MultipartFormDataContent();

            form.Add(
                new StringContent(dto.Name),
                "Name");

            form.Add(
                new StringContent(dto.Designer),
                "Designer");

            form.Add(
                new StringContent(dto.Category),
                "Category");

            form.Add(
                new StringContent(dto.Style),
                "Style");

            form.Add(
                new StringContent(dto.Size),
                "Size");

            form.Add(
                new StringContent(dto.PricePerDay.ToString()),
                "PricePerDay");

            form.Add(
                new StringContent(dto.IsAvailable.ToString()),
                "IsAvailable");

            if (dto.ImageFile != null)
            {
                var stream =
                    dto.ImageFile.OpenReadStream();

                var fileContent =
                    new StreamContent(stream);

                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(
                        dto.ImageFile.ContentType);

                form.Add(
                    fileContent,
                    "ImageFile",
                    dto.ImageFile.FileName);
            }

            var response =
                await client.PostAsync(
                    "api/FashionItems",
                    form);

            return response.IsSuccessStatusCode;
        }
        // UPDATE
        public async Task<bool> Update(
            int id,
            UpdateFashionItemDto dto)
        {
            var client = CreateClient();

            var form = new MultipartFormDataContent();

            form.Add(
                new StringContent(dto.Name),
                "Name");

            form.Add(
                new StringContent(dto.Designer),
                "Designer");

            form.Add(
                new StringContent(dto.Category),
                "Category");

            form.Add(
                new StringContent(dto.Style),
                "Style");

            form.Add(
                new StringContent(dto.Size),
                "Size");

            form.Add(
                new StringContent(dto.PricePerDay.ToString()),
                "PricePerDay");

            form.Add(
                new StringContent(dto.IsAvailable.ToString()),
                "IsAvailable");

            form.Add(
                new StringContent(dto.ImageUrl ?? ""),
                "ImageUrl");

            if (dto.ImageFile != null)
            {
                var stream =
                    dto.ImageFile.OpenReadStream();

                var fileContent =
                    new StreamContent(stream);

                form.Add(
                    fileContent,
                    "ImageFile",
                    dto.ImageFile.FileName);
            }

            var request =
                new HttpRequestMessage(
                    HttpMethod.Put,
                    $"api/FashionItems/{id}")
                {
                    Content = form
                };

            var response =
                await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> Delete(int id)
        {
            var client = CreateClient();

            var response =
                await client.DeleteAsync(
                    $"api/FashionItems/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}