using HCI.Models.Pictures.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI.Models.Pictures.Service
{
    public interface IPictureService
    {
        bool AddPicture(string base64Picture);
        List<Picture> GetAllPictures();
        Picture GetPictureById(int id);
        void DeletePicture(int id);
    }
}
