using HCI.Models.Attractions.Model;

namespace HCI.Models.Attractions.DTO
{
    public class EditAttracitonArgs
    {
        public Attraction attraction;

        public EditAttracitonArgs(Attraction attraction)
        {
            this.attraction = attraction;
        }
    }
}