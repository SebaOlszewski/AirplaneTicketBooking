using Data.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class FlightDbRepository
    {
        public AirlineDbContext _AirlineDbContext;
        public FlightDbRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }

        public void AddFlight(Flight newFlight)
        {
            _AirlineDbContext.Flights.Add(newFlight);
            _AirlineDbContext.SaveChanges();
        }


        public Flight? GetFlight(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId);
        }

        public IQueryable<Flight> GetFlights()
        {
            return _AirlineDbContext.Flights;
        }


    }
}
