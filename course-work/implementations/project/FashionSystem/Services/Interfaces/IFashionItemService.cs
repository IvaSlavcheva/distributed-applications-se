using FashionSystem.Models.FashionItem;

namespace FashionSystem.Services.Interfaces
{
    public interface IFashionItemService
    {
        Task<IEnumerable<GetFashionItemDto>> GetAll();

        Task<GetFashionItemDto?> GetById(int id);

        Task<bool> Create(CreateFashionItemDto dto);

        Task<bool> Update(int id, UpdateFashionItemDto dto);

        Task<bool> Delete(int id);
    }
}
