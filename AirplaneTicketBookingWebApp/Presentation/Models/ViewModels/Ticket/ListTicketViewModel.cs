using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Ticket
{
    public class ListTicketViewModel
    {
        public FlightDbRepository flightRepository;
        public ListTicketViewModel() { }


        public ListTicketViewModel(Guid chosenSeatId, ISeatInterface seatRepository)
        {
            Seat = seatRepository.GetSeats().Where(x => x.Id == chosenSeatId); //Get the seat object
            Guid flightFK = seatRepository.GetSeat(chosenSeatId).FlightFk;
            CountryTo = 
            CountryFrom = flightRepository.GetFlight(flightFK).CountryFrom;
        }

        public Guid Id { get; set; }

        [ForeignKey("Seat")]
        public Guid SeatFk { get; set; }       //Foreigh key

        public IQueryable<Seat> Seat { get; set; }  //Navigational



        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }

        public string Owner { get; set; }

        public string PassportImage { get; set; }

        public double PricePaid { get; set; }
        public bool Cancelled { get; set; } = false;

    }
}
