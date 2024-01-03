using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace Presentation.Models.ViewModels.Tickets
{
    public class EditTicketViewModel
    {
        private ITicketRepository flightsRepository;
        private IFlightRepository flightRepository;

        public Guid Id { get; set; } //we need to know which ticket to edit

        public EditTicketViewModel() { }
        public EditTicketViewModel(IFlightRepository flightRepository)
        {
            Flights = flightRepository.GetFlights(); //populate the list of flights

        }
        public int Row { get; set; }
        public int Column { get; set; }

        public int maxColumn { get; set; }
        public int maxRow { get; set; }

        public IQueryable<Flight> Flights { get; set; }
        public Guid FlightFK { get; set; }       //Foreigh key
        public string? Image { get; set; }
        public IFormFile PassportImage { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }
       
    }
}
