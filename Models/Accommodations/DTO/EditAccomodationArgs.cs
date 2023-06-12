using HCI.Models.Accommodations.Model;

namespace HCI.Models.Accommodations.DTO
{
    public class EditAccomodationArgs
    {
        public Accommodation accomodation;

        public EditAccomodationArgs(Accommodation accomodation)
        {
            this.accomodation = accomodation;
        }
    }
}