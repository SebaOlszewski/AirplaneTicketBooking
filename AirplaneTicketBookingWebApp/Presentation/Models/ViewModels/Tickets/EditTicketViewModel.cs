using Data.Repositories;
using Domain.Models;

namespace Presentation.Models.ViewModels.Tickets
{
    public class EditTicketViewModel
    {
        public Guid Id { get; set; } //we need to know which ticket to edit

        public EditTicketViewModel() { }
        public EditTicketViewModel(FlightDbRepository flightRepository)
        {
            Flights = flightRepository.GetFlights(); //populate the list of Categories

        }
        public int Row { get; set; }
        public int Column { get; set; }

        public IQueryable<Flight> Flights { get; set; }
        public Guid FlightFK { get; set; }       //Foreigh key
        public string? Image { get; set; }
        public IFormFile PassportImage { get; set; }
        public double PricePaid { get; set; }

        public bool Cancelled { get; set; }
    }
}
