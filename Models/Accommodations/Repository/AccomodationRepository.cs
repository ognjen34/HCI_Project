using HCI.Models.Accommodations.Model;
using HCI.Models.Attractions.Model;
using HCI.Models.Trips.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Accommodations.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AppDbContext _dbContext;

        public AccommodationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Accommodation GetById(int id)
        {
            return _dbContext.Accommodations.Find(id);
        }

        public void Add(Accommodation accommodation)
        {
            _dbContext.Accommodations.Add(accommodation);
            _dbContext.SaveChanges();
        }

        public void Remove(Accommodation accommodation)
        {
            accommodation.IsDeleted = true;
            List<Trip> tripsWithHotel = _dbContext.Trips.Where(trip => trip.Accommodation.Id == accommodation.Id).ToList();
            foreach(Trip trip in tripsWithHotel)
            {
                trip.IsDeleted = true;
            }
            _dbContext.SaveChanges();
        }

        public IEnumerable<Accommodation> GetAll()
        {
            return _dbContext.Accommodations.Where(r => !r.IsDeleted).ToList();
        }
        public void Update(Accommodation accommodation)
        {
            var existingAccommodation = _dbContext.Accommodations.FirstOrDefault(a => a.Id == accommodation.Id);
            if (existingAccommodation != null)
            {
                existingAccommodation.Name = accommodation.Name;
                existingAccommodation.PricePerDay = accommodation.PricePerDay;
                existingAccommodation.Location = accommodation.Location;
                existingAccommodation.Description = accommodation.Description;
                existingAccommodation.Pictures = accommodation.Pictures;
                _dbContext.SaveChanges();
            }
        }
    }
}
