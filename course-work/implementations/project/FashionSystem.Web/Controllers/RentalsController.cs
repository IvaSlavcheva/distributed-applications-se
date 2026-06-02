using FashionSystem.Web.Models.Rental;
using FashionSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FashionSystem.Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly RentalService _rentalService;

        public RentalsController(
            RentalService rentalService)
        {
            _rentalService = rentalService;
        }

        // GET: Rentals/MyRentals
        public async Task<IActionResult> MyRentals()
        {
            var userId =
                HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var rentals =
                await _rentalService
                    .GetUserRentals(
                        int.Parse(userId));

            return View(rentals);
        }

        // GET: Rentals/Rent/5
        [HttpGet]
        public IActionResult Rent(int id)
        {
            var model = new CreateRentalDto
            {
                FashionItemId = id,
                RentDate = DateTime.Today,
                ReturnDate = DateTime.Today.AddDays(1)
            };

            return View(model);
        }

        // POST: Rentals/Rent
        [HttpPost]
        public async Task<IActionResult> Rent(
            CreateRentalDto model)
        {
            var userId =
                HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = int.Parse(userId);

            var result =
                await _rentalService.Create(model);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message);

                return View(model);
            }

            return RedirectToAction(nameof(MyRentals));
        }

        // POST: Rentals/ReturnRental/5
        [HttpPost]
        public async Task<IActionResult> ReturnRental(int id)
        {
            var result =
                await _rentalService.ReturnRental(id);

            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(MyRentals));
        }
    }
}