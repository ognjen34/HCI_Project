using HCI.Models.Pictures.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Pictures.Repository
{
    public class PictureRepository : IPictureRepository
    {
        private readonly AppDbContext _dbContext;

        public PictureRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPicture(Picture picture)
        {
            _dbContext.Pictures.Add(picture);
            _dbContext.SaveChanges();
        }

        public List<Picture> GetAllPictures()
        {
            return _dbContext.Pictures.ToList();
        }

        public Picture GetPictureById(int id)
        {
            return _dbContext.Pictures.Find(id);
        }

        public void DeletePicture(int id)
        {
            var picture = _dbContext.Pictures.Find(id);
            if (picture != null)
            {
                _dbContext.Pictures.Remove(picture);
                _dbContext.SaveChanges();
            }
        }
    }
}
