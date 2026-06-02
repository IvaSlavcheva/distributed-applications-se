using FashionSystem.Entities;
using FashionSystem.Models.FashionItem;
using FashionSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FashionSystem.Models;

namespace FashionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FashionItemsController : ControllerBase
    {
        private readonly IRepository<FashionItem> _fashionItemRepository;

        public FashionItemsController(IRepository<FashionItem> fashionItemRepository)
        {
            _fashionItemRepository = fashionItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetFashionItemDto>>> GetAll(
            string? designer,
            string? category,
            string? style,
            string? sortBy,
            int page = 1,
            int pageSize = 10)
        {
            var fashionItems = await _fashionItemRepository.GetAll();

            if (!string.IsNullOrEmpty(designer))
            {
                fashionItems = fashionItems.Where(f =>
                    f.Designer.ToLower().Contains(designer.ToLower()));
            }

            if (!string.IsNullOrEmpty(category))
            {
                fashionItems = fashionItems.Where(f =>
                    f.Category.ToLower().Contains(category.ToLower()));
            }

            if (!string.IsNullOrEmpty(style))
            {
                fashionItems = fashionItems.Where(f =>
                    f.Style.ToLower().Contains(style.ToLower()));
            }

            fashionItems = sortBy?.ToLower() switch
            {
                "price" => fashionItems.OrderBy(f => f.PricePerDay),
                "designer" => fashionItems.OrderBy(f => f.Designer),
                "date" => fashionItems.OrderByDescending(f => f.CreatedAt),
                _ => fashionItems
            };

            var totalItems = fashionItems.Count();

            fashionItems = fashionItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var items = fashionItems.Select(f => new GetFashionItemDto
            {
                Id = f.Id,
                Name = f.Name,
                Designer = f.Designer,
                Category = f.Category,
                Style = f.Style,
                Size = f.Size,
                PricePerDay = f.PricePerDay,
                IsAvailable = f.IsAvailable,
                ImageUrl = f.ImageUrl,
                CreatedAt = f.CreatedAt
            }).ToList();

            return Ok(new PagedResult<GetFashionItemDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetFashionItemDto>> GetById(int id)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return NotFound();
            }

            var result = new GetFashionItemDto
            {
                Id = fashionItem.Id,
                Name = fashionItem.Name,
                Designer = fashionItem.Designer,
                Category = fashionItem.Category,
                Style = fashionItem.Style,
                Size = fashionItem.Size,
                PricePerDay = fashionItem.PricePerDay,
                IsAvailable = fashionItem.IsAvailable,
                ImageUrl = fashionItem.ImageUrl,
                CreatedAt = fashionItem.CreatedAt
            };

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromForm] CreateFashionItemDto dto)
        {
            string imageUrl = "";

            if (dto.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "FashionSystem.Web",
                    "wwwroot",
                    "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(dto.ImageFile.FileName);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                imageUrl = $"/images/{fileName}";
            }

            var fashionItem = new FashionItem
            {
                Name = dto.Name,
                Designer = dto.Designer,
                Category = dto.Category,
                Style = dto.Style,
                Size = dto.Size,
                PricePerDay = dto.PricePerDay,
                IsAvailable = dto.IsAvailable,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _fashionItemRepository.Create(fashionItem);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(fashionItem);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromForm] UpdateFashionItemDto dto)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return NotFound();
            }

            fashionItem.Name = dto.Name;
            fashionItem.Designer = dto.Designer;
            fashionItem.Category = dto.Category;
            fashionItem.Style = dto.Style;
            fashionItem.Size = dto.Size;
            fashionItem.PricePerDay = dto.PricePerDay;
            fashionItem.IsAvailable = dto.IsAvailable;

            if (dto.ImageFile != null)
            {
                var allowedExtensions =
                    new[] { ".jpg", ".jpeg", ".png", ".webp" };

                var extension =
                    Path.GetExtension(dto.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest("Only JPG, PNG and WEBP images are allowed.");
                }

                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..",
                    "FashionSystem.Web",
                    "wwwroot",
                    "images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                fashionItem.ImageUrl = $"/images/{fileName}";
            }

            await _fashionItemRepository.Update(fashionItem);

            return Ok(fashionItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return NotFound();
            }

            var result = await _fashionItemRepository.Delete(fashionItem);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}