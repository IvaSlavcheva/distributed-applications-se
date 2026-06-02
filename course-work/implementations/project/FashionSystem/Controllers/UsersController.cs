using FashionSystem.Entities;
using FashionSystem.Models.User;
using FashionSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FashionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UsersController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetAll()
        {
            var users = await _userRepository.GetAll();

            var result = users.Select(u => new GetUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                PhoneNumber = u.PhoneNumber,
                CreatedAt = u.CreatedAt
            });

            return Ok(result);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetById(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = new GetUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };

            return Ok(result);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult> Create(CreateUserDto dto)
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

            var result = await _userRepository.Create(user);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.PhoneNumber = dto.PhoneNumber;

            var result = await _userRepository.Update(user);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userRepository.Delete(user);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}