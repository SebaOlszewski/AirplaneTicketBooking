using Data.Repositories;
using Domain.Models;
using Presentation.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Ticket
{
    public class BookTicketViewModel
    {

                

        
        public Guid SeatFk { get; set; }
        [Display(Name = "Passport:")]
        public IFormFile PassportImage { get; set; }
        [Display(Name = "Email: ")]
        public string Owner { get; set; } = "";

        public Guid chosenFlight { get; set; }
        public List<Seat> seatingList { get; set; }

        [Display(Name = "Ticket price:")]
        public double PricePaid { get; set; }

        public int maxRowLength { get; set; }
        public int maxColLength { get; set; }

        


    }
}
