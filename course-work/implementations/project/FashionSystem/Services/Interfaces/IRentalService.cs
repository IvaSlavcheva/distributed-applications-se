using FashionSystem.Models.Rental;

namespace FashionSystem.Services.Interfaces
{
    public interface IRentalService
    {
        Task<IEnumerable<GetRentalDto>> GetAll();

        Task<GetRentalDto?> GetById(int id);

        Task<bool> Create(CreateRentalDto dto);

        Task<bool> Update(int id, UpdateRentalDto dto);

        Task<bool> Delete(int id);
    }
}
