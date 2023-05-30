using HCI.Models.Pictures.Model;
using HCI.Models.Pictures.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Pictures.Service
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;

        public PictureService(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }

        public bool AddPicture(string base64Picture)
        {
            // Validate if the base64Picture is a valid base64 string
            if (!IsValidBase64String(base64Picture))
            {
                return false;
            }

            // Convert base64 string to byte array or store it as a string, as required
            var picture = new Picture
            {
                Pictures = base64Picture
            };

            _pictureRepository.AddPicture(picture);
            return true;
        }

        private bool IsValidBase64String(string base64String)
        {
            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public List<Picture> GetAllPictures()
        {
            return _pictureRepository.GetAllPictures();
        }

        public Picture GetPictureById(int id)
        {
            return _pictureRepository.GetPictureById(id);
        }

        public void DeletePicture(int id)
        {
            _pictureRepository.DeletePicture(id);
        }
    }

}
