using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Data.DataContext;

namespace Data.Repositories
{

    public class FlightsJsonRepository : IFlightRepository
    {
        public IFlightRepository _AirlineDbContext;
        public FlightsJsonRepository(IFlightRepository AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }


        private string _path;
        public FlightsJsonRepository(string path)
        {
            _path = path;
            if (File.Exists(_path) == false)
            {
                File.Create(_path).Close();
            }

        }

        public void AddFlight(Flight newFlight)
        {
            newFlight.Id = Guid.NewGuid();
            var list = GetFlights().ToList();
            list.Add(newFlight);

            var allFLightsText = JsonSerializer.Serialize(list);

            try
            {
                File.WriteAllText(_path, allFLightsText);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding a flight");
            }



        }

        public void DeleteFlight(Guid FlightId)
        {
            var list = GetFlights().ToList();
            var flightToDelete = list.FirstOrDefault(f => f.Id == FlightId);

           

            if (flightToDelete != null)
            {
                try
                {
                    list.Remove(flightToDelete);
                    var allFLightsText = JsonSerializer.Serialize(list);
                    File.WriteAllText(_path, allFLightsText);
                   
                }catch(Exception ex)
                {
                    throw new Exception("Error while deleteting a flight");
                }

            }else
            {
                throw new Exception("Flight not found");
            }

        }

        public string getCountryFrom(Guid FlightId)
        {
            var list = GetFlights().ToList();
            var chosenFlightCountryFrom = list.FirstOrDefault(f => f.Id == FlightId).CountryFrom;

            return chosenFlightCountryFrom;
        }

        public string getCountryTo(Guid FlightId)
        {
            var list = GetFlights().ToList();
            var chosenFlightCountryTo = list.FirstOrDefault(f => f.Id == FlightId).CountryTo;

            return chosenFlightCountryTo;
        }

        public Flight? GetFlights(Guid FlightId)
        {
            return GetFlights().SingleOrDefault(x => x.Id == FlightId);
        }

        public IQueryable<Flight> GetFlights()
        {
            //checking if file exists
            if (File.Exists(_path))
            {
                try
                {

                    string allText = "";
                    //reading the data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    //return empty list if file was empty
                    if (string.IsNullOrEmpty(allText))
                    {
                        return new List<Flight>().AsQueryable();
                    }
                    else
                    {

                        List<Flight> myFlights = JsonSerializer.Deserialize<List<Flight>>(allText); //converts from json string to an object
                        return myFlights.AsQueryable();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("Error while opening the file");
                }
            }
            else throw new Exception("File saving products not found!");
        }

        public void UpdateFlight(Flight chosenFlight)
        {

            var list = GetFlights().ToList();
            var flightToUpdate = list.FirstOrDefault(f => f.Id == chosenFlight.Id);

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
                try
                {
                    var allFlightsText = JsonSerializer.Serialize(list);
                    File.WriteAllText(_path, allFlightsText);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating a flight", ex);
                }
            }
            else
            {
                throw new Exception($"Flight with ID {chosenFlight.Id} not found.");
            }


        }

    }
}
