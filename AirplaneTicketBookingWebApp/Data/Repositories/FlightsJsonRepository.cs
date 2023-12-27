using Domain.Interfaces;
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

namespace Data.Repositories
{

    public class FlightsJsonRepository
    {
        private string _path;
        public FlightsJsonRepository(string path)
        {
            _path = path;
            if(File.Exists(_path) == false)
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
            }catch(Exception ex)
            {
                throw new Exception("Error while adding a flight");
            }
            


        }

        public void DeleteFlight(Guid FlightId)
        {
            throw new NotImplementedException();
        }

        public string getCountryFrom(Guid FlightId)
        {
            throw new NotImplementedException();
        }

        public string getCountryTo(Guid FlightId)
        {
            throw new NotImplementedException();
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
                catch(Exception ex)
                {
                    throw new Exception("Error while opening the file");
                }
            }
            else throw new Exception("File saving products not found!");
        }

        public void UpdateFlight(Flight chosenFlight)
        {
            throw new NotImplementedException();
        }
    }
}
