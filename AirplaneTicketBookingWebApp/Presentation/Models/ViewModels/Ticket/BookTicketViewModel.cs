using Data.Repositories;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Ticket
{
    public class BookTicketViewModel
    {

        public BookTicketViewModel() { }

        /*

        public BookTicketViewModel(Guid chosenFlightId, SeatDbRepository seatRepository)
        {
            Seats = seatRepository.GetAllTheSeatsFromAFlight(chosenFlightId); //populate the list of seats
        }
        public Guid SeatFk { get; set; }       //Foreigh key

        public IQueryable<Seat> Seats { get; set; }  //Navigational
        */

        public Guid SeatFk { get; set; }
        public IFormFile PassportImage { get; set; }

        public string Owner { get; set; } = "";

        public Guid chosenFlight { get; set; }
        public List<Seat> seatingList { get; set; }

        public double PricePaid { get; set; }

        public int maxRowLength { get; set; }
        public int maxColLength { get; set; }

        


    }
}
