using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Tickets
{
    public class BookTicketViewModel
    {
        private IFlights flightsRepository;

        public Guid Id { get; set; }
        public BookTicketViewModel(IFlights flightsRepository)
        {
            Flights = flightsRepository.GetFlights(); //populate the list of Categories
        }

        public int Row { get; set; }
        public int Column { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightFK { get; set; }       //Foreigh key
        public IQueryable<Flight> Flights { get; set; }
        public string? Image { get; set; }
        public IFormFile PassportImage { get; set; }
        public double PricePaid { get; set; }

        public bool Cancelled { get; set; } = false;
    }
}
