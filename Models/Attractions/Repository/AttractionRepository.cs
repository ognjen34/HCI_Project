using HCI.Models.Attractions.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HCI.Models.Attractions.Repository
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly AppDbContext dbContext;

        public AttractionRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Attraction GetById(int id)
        {
            return dbContext.Attractions.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Attraction> GetAll()
        {
            return dbContext.Attractions.ToList();
        }

        public void Add(Attraction attraction)
        {
            attraction.ClassName = "Attraction";
            dbContext.Attractions.Add(attraction);
            dbContext.SaveChanges();
        }

        public void Update(Attraction attraction)
        {
            var existingAttraction = dbContext.Attractions.FirstOrDefault(a => a.Id == attraction.Id);
            if (existingAttraction != null)
            {
                existingAttraction.Name = attraction.Name;
                existingAttraction.Price = attraction.Price;
                existingAttraction.Location = attraction.Location;
                existingAttraction.Description = attraction.Description;
                dbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var attraction = dbContext.Attractions.FirstOrDefault(a => a.Id == id);
            if (attraction != null)
            {
                dbContext.Attractions.Remove(attraction);
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Attraction> GetAllFromCity(string city)
        {
            return dbContext.Attractions.Where(a => a.Location.City == city).ToList();
        }
    }
}