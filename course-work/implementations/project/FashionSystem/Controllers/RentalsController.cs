using FashionSystem.Entities;
using FashionSystem.Models.Rental;
using FashionSystem.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FashionSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRepository<Rental> _rentalRepository;
        private readonly IRepository<FashionItem> _fashionItemRepository;

        public RentalsController(
            IRepository<Rental> rentalRepository,
            IRepository<FashionItem> fashionItemRepository)
        {
            _rentalRepository = rentalRepository;
            _fashionItemRepository = fashionItemRepository;
        }

        // GET: api/rentals/user/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>>
            GetUserRentals(int userId)
        {
            var rentals =
                await _rentalRepository.GetAll();

            var fashionItems =
                await _fashionItemRepository.GetAll();

            var result = rentals
                .Where(r => r.UserId == userId)
                .Select(r =>
                {
                    var item = fashionItems
                        .FirstOrDefault(f =>
                            f.Id == r.FashionItemId);

                    return new
                    {
                        Id = r.Id,
                        FashionItemId = r.FashionItemId,
                        FashionItemName =
                            item?.Name ?? "Unknown",
                        ImageUrl =
                            item?.ImageUrl,
                        RentDate = r.RentDate,
                        ReturnDate = r.ReturnDate,
                        TotalPrice = r.TotalPrice,
                        Status = r.Status
                    };
                });

            return Ok(result);
        }

        // GET: api/rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRentalDto>>> GetAll()
        {
            var rentals = await _rentalRepository.GetAll();

            var result = rentals.Select(r => new GetRentalDto
            {
                Id = r.Id,
                UserId = r.UserId,
                FashionItemId = r.FashionItemId,
                RentDate = r.RentDate,
                ReturnDate = r.ReturnDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            });

            return Ok(result);
        }

        // GET: api/rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRentalDto>> GetById(int id)
        {
            var rental = await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return NotFound();
            }

            var result = new GetRentalDto
            {
                Id = rental.Id,
                UserId = rental.UserId,
                FashionItemId = rental.FashionItemId,
                RentDate = rental.RentDate,
                ReturnDate = rental.ReturnDate,
                TotalPrice = rental.TotalPrice,
                Status = rental.Status,
                CreatedAt = rental.CreatedAt
            };

            return Ok(result);
        }

        // POST: api/rentals
        [HttpPost]
        public async Task<ActionResult> Create(CreateRentalDto dto)
        {
            var fashionItem =
                await _fashionItemRepository
                    .GetById(dto.FashionItemId);

            if (fashionItem == null)
            {
                return NotFound("Fashion item not found.");
            }

            // CHECK IF PRODUCT IS ALREADY RENTED
            if (!fashionItem.IsAvailable)
            {
                return BadRequest(
                    "Fashion item is already rented.");
            }

            // Validate dates
            if (dto.RentDate > dto.ReturnDate)
            {
                return BadRequest(
                    "Return date must be after rent date.");
            }

            if (dto.RentDate < DateTime.UtcNow.Date)
            {
                return BadRequest(
                    "Rent date cannot be in the past.");
            }

            var rentals =
                await _rentalRepository.GetAll();

            // Check overlapping rentals
            var isBooked = rentals.Any(r =>
                r.FashionItemId == dto.FashionItemId &&
                r.Status != "Returned" &&
                dto.RentDate < r.ReturnDate &&
                dto.ReturnDate > r.RentDate);

            if (isBooked)
            {
                return BadRequest(
                    "Fashion item is already rented for the selected period.");
            }

            // Calculate total days
            var totalDays =
                (dto.ReturnDate - dto.RentDate).Days + 1;

            if (totalDays <= 0)
            {
                return BadRequest(
                    "Invalid rental period.");
            }

            // Calculate total price
            decimal totalPrice =
                totalDays * fashionItem.PricePerDay;

            var rental = new Rental
            {
                UserId = dto.UserId,
                FashionItemId = dto.FashionItemId,
                RentDate = dto.RentDate,
                ReturnDate = dto.ReturnDate,
                TotalPrice = totalPrice,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            var result =
                await _rentalRepository.Create(rental);

            if (!result)
            {
                return BadRequest();
            }

            // PRODUCT BECOMES RENTED
            fashionItem.IsAvailable = false;

            await _fashionItemRepository.Update(fashionItem);

            return Ok(
                "Fashion item rented successfully.");
        }

        // PUT: api/rentals/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(
            int id,
            UpdateRentalDto dto)
        {
            var rental =
                await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return NotFound();
            }

            rental.UserId = dto.UserId;
            rental.FashionItemId = dto.FashionItemId;
            rental.RentDate = dto.RentDate;
            rental.ReturnDate = dto.ReturnDate;
            rental.TotalPrice = dto.TotalPrice;
            rental.Status = dto.Status;

            var result =
                await _rentalRepository.Update(rental);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(
                "Rental updated successfully.");
        }

        // PUT: api/rentals/5/return
        [HttpPut("{id}/return")]
        public async Task<ActionResult> ReturnRental(int id)
        {
            var rental =
                await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return NotFound();
            }

            rental.Status = "Returned";

            var result =
                await _rentalRepository.Update(rental);

            if (!result)
            {
                return BadRequest();
            }

            // PRODUCT BECOMES AVAILABLE AGAIN
            var fashionItem =
                await _fashionItemRepository
                    .GetById(rental.FashionItemId);

            if (fashionItem != null)
            {
                fashionItem.IsAvailable = true;

                await _fashionItemRepository.Update(fashionItem);
            }

            return Ok(
                "Fashion item returned successfully.");
        }

        // DELETE: api/rentals/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var rental =
                await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return NotFound();
            }

            var result =
                await _rentalRepository.Delete(rental);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}