using Data.Repositories;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Tickets
{
    public class ListTicketsViewModel
    {
        public Guid Id { get; set; }

        public ListTicketsViewModel() { }
        public ListTicketsViewModel(FlightDbRepository flightRepository)
        {
            Flights = flightRepository.GetFlights(); //populate the list of Categories

        }

        public int Row { get; set; }
        public int Column { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightFK { get; set; }

        public IQueryable<Flight> Flights { get; set; }
        
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public string Passport { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }

    }
}
