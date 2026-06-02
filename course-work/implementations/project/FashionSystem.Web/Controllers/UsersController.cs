using FashionSystem.Web.Models.User;
using FashionSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FashionSystem.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            dto.Role = "User";

            var result =
                await _userService.Register(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to register");

                return View(dto);
            }

            return RedirectToAction(
                "Login",
                "Auth");
        }
    }
}