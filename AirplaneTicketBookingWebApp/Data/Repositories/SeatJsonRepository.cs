using Data.DataContext;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SeatJsonRepository : ISeatInterface
    {
        public AirlineDbContext _AirlineDbContext;
        public SeatJsonRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }

        private string _path;
        public SeatJsonRepository(string path)
        {
            _path = path;

            if (File.Exists(_path) == false)
            {
                using (FileStream fs = File.Create(path))
                {

                    fs.Close();
                }
            }


        }
        public void AddSeat(Seat newSeat)
        {
            try
            {
                newSeat.Id = Guid.NewGuid();

                var list = GetSeats().ToList();
                list.Add(newSeat);

                var allSeatsText = JsonSerializer.Serialize(list);

            
                File.WriteAllText(_path, allSeatsText);

            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding product!");
            }
        }

        public void deleteChosenSeatInAFlight(Guid flightId, int col, int row)
        {
            if (File.Exists(_path))      //checking if file exists
            {
                try
                {
                    string allText = "";
                    //read data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    if (string.IsNullOrEmpty(allText))
                    {
                        throw new Exception("Error while deleteing the product");
                    }
                    else
                    {
                        var mySeats = JsonSerializer.Deserialize<List<Seat>>(allText);                                                      //converts from Json string into an object (list of products)
                        var objectsToRemove = mySeats.Where(item => item.Id == flightId && item.Column == col && item.Row == row);        //create the list of seats with the same id, should be only one
                        foreach (var objectToRemove in objectsToRemove)                                                                       // go through all of them and delete them
                        {
                            mySeats.Remove(objectToRemove);
                        }
                        string updatedJson = JsonSerializer.Serialize(mySeats, new JsonSerializerOptions { WriteIndented = true });

                    }
                }catch
                {
                    throw new Exception("Error while opening the file");
                }

            }else
            {
                throw new Exception("Error while opening the file");
            }
        }

        public void deleteSeat(Guid seatId)
        {
            if (File.Exists(_path))      //checking if file exists
            {
                try
                {
                    string allText = "";
                    //read data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    if (string.IsNullOrEmpty(allText))
                    {
                        throw new Exception("Error while deleteing the product");
                    }
                    else
                    {
                        var mySeats = JsonSerializer.Deserialize<List<Seat>>(allText);  //converts from Json string into an object (list of products)
                        var objectsToRemove = mySeats.Where(item => item.Id == seatId);        //create the list of seats with the same id, should be only one
                        foreach (var objectToRemove in objectsToRemove)                       // go through all of them and delete them
                        {
                            mySeats.Remove(objectToRemove);
                        }
                        string updatedJson = JsonSerializer.Serialize(mySeats, new JsonSerializerOptions { WriteIndented = true });

                    }
                }
                catch
                {
                    throw new Exception("Error while opening the file");
                }

            }
            else
            {
                throw new Exception("Error while opening the file");
            }
        }

        public IQueryable<Seat> GetAllTheSeatsFromAFlight(Guid flightId)
        {
            if (File.Exists(_path))      //checking if file exists
            {
                try
                {
                    string allText = "";
                    //read data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    //deserialazie takes the json file and transforms into an object
                    //serialize does the other way aound

                    //if file is empty we return empty list
                    if (string.IsNullOrEmpty(allText))
                    {
                        return new List<Seat>().AsQueryable();

                    }

                    List<Seat> myProducts = JsonSerializer.Deserialize<List<Seat>>(allText);  //converts from Json string into an object (list of products)
                    return myProducts.AsQueryable();

                }
                catch (Exception ex)
                {
                    //log...
                    throw new Exception("Error while opening the file");
                }


            }
            else
            {
                throw new Exception("File saving products not found");
            }
        }

        public int getMaxColumnsFromAFlight(Guid flightID)
        {
            var seatsInFlight = GetAllTheSeatsFromAFlight(flightID);
            int maxColumn = seatsInFlight.Max(x => x.Column);
            return maxColumn;
        }

        public int getMaxRowsFromAFlight(Guid flightID)
        {
            var seatsInFlight = GetAllTheSeatsFromAFlight(flightID);
            int maxRow = seatsInFlight.Max(x => x.Row);
            return maxRow;
        }

        public Seat? GetSeat(Guid seatId)
        {
            return GetSeats().SingleOrDefault(x => x.Id == seatId);
        }

        public IQueryable<Seat> GetSeats()
        {
            if (File.Exists(_path))      //checking if file exists
            {
                try
                {
                    string allText = "";
                    //read data from the file
                    using (StreamReader sr = File.OpenText(_path))
                    {
                        allText = sr.ReadToEnd();
                        sr.Close();
                    }

                    //deserialazie takes the json file and transforms into an object
                    //serialize does the other way aound

                    //if file is empty we return empty list
                    if (string.IsNullOrEmpty(allText))
                    {
                        return new List<Seat>().AsQueryable();

                    }

                    List<Seat> mySeats = JsonSerializer.Deserialize<List<Seat>>(allText);  //converts from Json string into an object (list of products)
                    return mySeats.AsQueryable();

                }
                catch (Exception ex)
                {
                    //log...
                    throw new Exception("Error while opening the file");
                }


            }
            else
            {
                throw new Exception("File saving products not found");
            }
        }

        public bool isSeatTaken(Guid seatId)
        {
            var seatToUpdate = GetSeat(seatId);

            if (seatToUpdate.isTaken == true)
            {
                return true;
            }
            return false;
        }

        public void takeSeat(Guid seatId)
        {
            var list = GetSeats().ToList();
            var chosenSeat = GetSeat(seatId);
            var seatToUpdate = list.FirstOrDefault(x => x.Id == seatId);

            if (seatToUpdate != null)
            {
                if(seatToUpdate.isTaken == true)
                {
                    seatToUpdate.isTaken = false;
                }else
                {
                    seatToUpdate.isTaken = true;
                }

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
                throw new Exception($"Flight with ID {seatId} not found.");
            }
        }

        public void updateSeat(Seat chosenSeat)
        {
            var list = GetSeats().ToList();
            var seatToUpdate = list.FirstOrDefault(x => x.Id == chosenSeat.Id);

            if (seatToUpdate != null)
            {
                seatToUpdate.Row = chosenSeat.Row;
                seatToUpdate.Column = chosenSeat.Column;
                seatToUpdate.FlightFk = chosenSeat.FlightFk;
                seatToUpdate.isTaken = chosenSeat.isTaken;
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
                throw new Exception($"Flight with ID {chosenSeat.Id} not found.");
            }
        }
    }
}
