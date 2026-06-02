using FashionSystem.Entities;
using FashionSystem.Models.Rental;
using FashionSystem.Repository;
using FashionSystem.Services.Interfaces;

namespace FashionSystem.Services
{
    public class RentalService: IRentalService
    {
        private readonly IRepository<Rental> _rentalRepository;

        public RentalService(IRepository<Rental> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<IEnumerable<GetRentalDto>> GetAll()
        {
            var rentals = await _rentalRepository.GetAll();

            return rentals.Select(r => new GetRentalDto
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
        }

        public async Task<GetRentalDto?> GetById(int id)
        {
            var rental = await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return null;
            }

            return new GetRentalDto
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
        }

        public async Task<bool> Create(CreateRentalDto dto)
        {
            var rental = new Rental
            {
                UserId = dto.UserId,
                FashionItemId = dto.FashionItemId,
                RentDate = dto.RentDate,
                ReturnDate = dto.ReturnDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            return await _rentalRepository.Create(rental);
        }

        public async Task<bool> Update(int id, UpdateRentalDto dto)
        {
            var rental = await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return false;
            }

            rental.UserId = dto.UserId;
            rental.FashionItemId = dto.FashionItemId;
            rental.RentDate = dto.RentDate;
            rental.ReturnDate = dto.ReturnDate;
            rental.TotalPrice = dto.TotalPrice;
            rental.Status = dto.Status;

            return await _rentalRepository.Update(rental);
        }

        public async Task<bool> Delete(int id)
        {
            var rental = await _rentalRepository.GetById(id);

            if (rental == null)
            {
                return false;
            }

            return await _rentalRepository.Delete(rental);
        }
    }
}
