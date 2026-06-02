using FashionSystem.Entities;
using FashionSystem.Models.FashionItem;
using FashionSystem.Repository;
using FashionSystem.Services.Interfaces;

namespace FashionSystem.Services
{
    public class FashionItemService: IFashionItemService
    {
        private readonly IRepository<FashionItem> _fashionItemRepository;

        public FashionItemService(IRepository<FashionItem> fashionItemRepository)
        {
            _fashionItemRepository = fashionItemRepository;
        }

        public async Task<IEnumerable<GetFashionItemDto>> GetAll()
        {
            var fashionItems = await _fashionItemRepository.GetAll();

            return fashionItems.Select(f => new GetFashionItemDto
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
            });
        }

        public async Task<GetFashionItemDto?> GetById(int id)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return null;
            }

            return new GetFashionItemDto
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
        }

        public async Task<bool> Create(CreateFashionItemDto dto)
        {
            var fashionItem = new FashionItem
            {
                Name = dto.Name,
                Designer = dto.Designer,
                Category = dto.Category,
                Style = dto.Style,
                Size = dto.Size,
                PricePerDay = dto.PricePerDay,
                IsAvailable = dto.IsAvailable,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            return await _fashionItemRepository.Create(fashionItem);
        }

        public async Task<bool> Update(int id, UpdateFashionItemDto dto)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return false;
            }

            fashionItem.Name = dto.Name;
            fashionItem.Designer = dto.Designer;
            fashionItem.Category = dto.Category;
            fashionItem.Style = dto.Style;
            fashionItem.Size = dto.Size;
            fashionItem.PricePerDay = dto.PricePerDay;
            fashionItem.IsAvailable = dto.IsAvailable;
            fashionItem.ImageUrl = dto.ImageUrl;

            return await _fashionItemRepository.Update(fashionItem);
        }

        public async Task<bool> Delete(int id)
        {
            var fashionItem = await _fashionItemRepository.GetById(id);

            if (fashionItem == null)
            {
                return false;
            }

            return await _fashionItemRepository.Delete(fashionItem);
        }
    }
}

