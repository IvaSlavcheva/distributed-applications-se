using FashionSystem.Web.Models.FashionItem;
using FashionSystem.Web.Models.Rental;
using FashionSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FashionSystem.Web.Controllers
{
    public class FashionItemsController : Controller
    {
        private readonly FashionItemService _fashionItemService;
        private readonly RentalService _rentalService;

        public FashionItemsController(
            FashionItemService fashionItemService,
            RentalService rentalService)
        {
            _fashionItemService = fashionItemService;
            _rentalService = rentalService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("JWT") != null;
        }

        // INDEX
        public async Task<IActionResult> Index(
            string? designer,
            string? category,
            string? sortBy,
            int page = 1)
        {
            var result =
                await _fashionItemService.GetAll(
                    designer,
                    category,
                    sortBy,
                    page,
                    8);

            return View(result);
        }
        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var item =
                await _fashionItemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // RENT
        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (!IsLoggedIn() || IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var userIdString =
                HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction(
                    "Login",
                    "Auth");
            }

            var item =
                await _fashionItemService.GetById(id);

            if (item == null || !item.IsAvailable)
            {
                return RedirectToAction(nameof(Index));
            }

            var dto = new CreateRentalDto
            {
                UserId = int.Parse(userIdString),
                FashionItemId = item.Id,
                RentDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(1),
            };

            var result =
                await _rentalService.Create(dto);

            if (!result.Success)
            {
                var updateDto = new UpdateFashionItemDto
                {
                    Name = item.Name,
                    Designer = item.Designer,
                    Category = item.Category,
                    Style = item.Style,
                    Size = item.Size,
                    PricePerDay = item.PricePerDay,
                    IsAvailable = false,
                    ImageUrl = item.ImageUrl
                };

                await _fashionItemService.Update(
                    item.Id,
                    updateDto);
            }

            return RedirectToAction(nameof(Index));
        }

        // CREATE
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateFashionItemDto dto)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result =
                await _fashionItemService.Create(dto);

            if (!result)
            {
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var item =
                await _fashionItemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            var dto = new UpdateFashionItemDto
            {
                Name = item.Name,
                Designer = item.Designer,
                Category = item.Category,
                Style = item.Style,
                Size = item.Size,
                PricePerDay = item.PricePerDay,
                IsAvailable = item.IsAvailable,
                ImageUrl = item.ImageUrl
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(
            int id,
            UpdateFashionItemDto dto)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var result =
                await _fashionItemService.Update(id, dto);

            if (!result)
            {
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var item =
                await _fashionItemService.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            await _fashionItemService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}