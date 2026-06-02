using FashionSystem.Models.User;

namespace FashionSystem.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<GetUserDto>> GetAll();

        Task<GetUserDto?> GetById(int id);

        Task<bool> Create(CreateUserDto dto);

        Task<bool> Update(int id, UpdateUserDto dto);

        Task<bool> Delete(int id);
    }
}
