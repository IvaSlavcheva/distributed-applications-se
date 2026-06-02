using FashionSystem.Web.Models.Auth;
using FashionSystem.Web.Models.User;
using FashionSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FashionSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;

        public AuthController(
            AuthService authService,
            UserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // LOGIN GET
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var token = await _authService.Login(dto);

            if (token == null)
            {
                ModelState.AddModelError(
                    "",
                    "Invalid email or password");

                return View(dto);
            }

            // SAVE JWT
            HttpContext.Session.SetString(
                "JWT",
                token);

            var handler =
                new JwtSecurityTokenHandler();

            var jwt =
                handler.ReadJwtToken(token);

            // ROLE
            var role = jwt.Claims
                .FirstOrDefault(c =>
                    c.Type.Contains("role"))
                ?.Value;

            // USER ID
            var userId = jwt.Claims
                .FirstOrDefault(c =>
                    c.Type.Contains("nameidentifier")
                    || c.Type.Contains("sub"))
                ?.Value;

            // SAVE ROLE
            if (role != null)
            {
                HttpContext.Session.SetString(
                    "Role",
                    role);
            }

            // SAVE USER ID
            if (userId != null)
            {
                HttpContext.Session.SetString(
                    "UserId",
                    userId);
            }

            return RedirectToAction(
                "Index",
                "Home");
        }

        // REGISTER GET
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER POST
        [HttpPost]
        public async Task<IActionResult> Register(
            CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result =
                await _userService.Register(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    "",
                    "Registration failed");

                return View(dto);
            }

            // AUTO LOGIN
            var loginDto = new LoginDto
            {
                Email = dto.Email,
                Password = dto.Password
            };

            var token =
                await _authService.Login(loginDto);

            if (token != null)
            {
                HttpContext.Session.SetString(
                    "JWT",
                    token);

                var handler =
                    new JwtSecurityTokenHandler();

                var jwt =
                    handler.ReadJwtToken(token);

                // ROLE
                var role = jwt.Claims
                    .FirstOrDefault(c =>
                        c.Type.Contains("role"))
                    ?.Value;

                // USER ID
                var userId = jwt.Claims
                    .FirstOrDefault(c =>
                        c.Type.Contains("nameidentifier")
                        || c.Type.Contains("sub"))
                    ?.Value;

                // SAVE ROLE
                if (role != null)
                {
                    HttpContext.Session.SetString(
                        "Role",
                        role);
                }

                // SAVE USER ID
                if (userId != null)
                {
                    HttpContext.Session.SetString(
                        "UserId",
                        userId);
                }
            }

            return RedirectToAction(
                "Index",
                "Home");
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(
                "Index",
                "Home");
        }
    }
}