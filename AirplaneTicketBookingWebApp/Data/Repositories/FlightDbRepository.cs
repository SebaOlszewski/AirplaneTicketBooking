﻿using Data.DataContext;
using Domain.Models;
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

        public string getCountryFrom(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId).CountryFrom;
        }

        public string getCountryTo(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId).CountryTo;
        }



        public void DeleteFlight(Guid FlightId)
        {
            var flightToDelete = GetFlight(FlightId);
            if (flightToDelete != null)
            {
                _AirlineDbContext.Remove(flightToDelete);
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No flight to delete");
            }
        }

        public void UpdateFlight(Flight chosenFlight)
        {
            var flightToUpdate = GetFlight(chosenFlight.Id);
            if (flightToUpdate != null)
            {
                flightToUpdate.Rows = chosenFlight.Rows;
                flightToUpdate.Columns = chosenFlight.Columns;
                flightToUpdate.DepartureDate = chosenFlight.DepartureDate;
                flightToUpdate.ArrivalDate = chosenFlight.ArrivalDate;
                flightToUpdate.CountryFrom = chosenFlight.CountryFrom;
                flightToUpdate.CountryTo = chosenFlight.CountryTo;
                flightToUpdate.WholesalePrice = chosenFlight.WholesalePrice;
                flightToUpdate.CommissionRate = chosenFlight.CommissionRate;
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No product to update");
            }
        }

        public int GetRows(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId).Rows;
        }

        public int GetColumns(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId).Columns;
        }


    }
}
