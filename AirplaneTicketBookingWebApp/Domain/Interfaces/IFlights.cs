using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IFlights
    {
        void AddFlight(Flight newFlight);

        Flight? GetFlights(Guid FlightId);

        IQueryable<Flight> GetFlights();

        string getCountryFrom(Guid FlightId);

        string getCountryTo(Guid FlightId);

        void DeleteFlight(Guid FlightId);

        void UpdateFlight(Flight chosenFlight);

    }
}
