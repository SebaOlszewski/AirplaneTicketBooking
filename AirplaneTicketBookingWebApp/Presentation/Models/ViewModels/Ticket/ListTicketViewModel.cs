using Data.Repositories;
using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Ticket
{
    public class ListTicketViewModel
    {

        public ListTicketViewModel() { }


        public ListTicketViewModel(Guid chosenSeatId,SeatDbRepository seatRepository)
        {
            Seat = seatRepository.GetSeats().Where(x => x.Id == chosenSeatId); //Get the seat object

        }

        public Guid Id { get; set; }

        [ForeignKey("Seat")]
        public Guid SeatFk { get; set; }       //Foreigh key

        public IQueryable<Seat> Seat { get; set; }  //Navigational

        public string Owner { get; set; }

        public string PassportImage { get; set; }

        public double PricePaid { get; set; }
        public bool Cancelled { get; set; } = false;

    }
}
