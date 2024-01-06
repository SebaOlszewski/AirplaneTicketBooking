using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels.Admin
{
    public class AdminListSeatsViewModel
    {
        public AdminListSeatsViewModel() { }

        public Guid chosenFlight { get; set; }
        public List<Seat> seatingList { get; set; }
        public int maxRowLength { get; set; }
        public int maxColLength { get; set; }


    }
}
