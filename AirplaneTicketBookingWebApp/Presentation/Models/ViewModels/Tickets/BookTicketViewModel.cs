using Data.Repositories;
using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Tickets
{
    public class BookTicketViewModel
    {
        public BookTicketViewModel() { }
        public BookTicketViewModel(FlightDbRepository flightRepository)
        {
            Flights = flightRepository.GetFlights(); //populate the list of Categories

        }
        public int Row { get; set; }
        public int Column { get; set; }

        public IQueryable<Flight> Flights { get; set; }
        public Guid FlightFK { get; set; }       //Foreigh key
        public string? Passport { get; set; }
        public double PricePaid { get; set; }


    }
}
