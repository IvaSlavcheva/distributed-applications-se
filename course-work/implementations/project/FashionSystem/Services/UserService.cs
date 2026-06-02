using FashionSystem.Entities;
using FashionSystem.Models.User;
using FashionSystem.Repository;
using FashionSystem.Services.Interfaces;

namespace FashionSystem.Services
{
    public class UserService: IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetUserDto>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return users.Select(u => new GetUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<GetUserDto?> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return null;
            }

            return new GetUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> Create(CreateUserDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            return await _userRepository.Create(user);
        }

        public async Task<bool> Update(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return false;
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.PhoneNumber = dto.PhoneNumber;

            return await _userRepository.Update(user);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return false;
            }

            return await _userRepository.Delete(user);
        }
    }
}

