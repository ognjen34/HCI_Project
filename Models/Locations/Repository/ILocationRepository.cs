using HCI.Models.Locations.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace HCI.Models.Locations.Repository
{
    public interface ILocationRepository
    {
        Task<Location> GetById(int id);
        Task<List<Location>> GetAll();
        Task Add(Location location);
        Task Update(Location location);
        Task Delete(int id);
    }
}