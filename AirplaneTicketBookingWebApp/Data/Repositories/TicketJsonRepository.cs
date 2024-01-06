using Data.DataContext;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TicketJsonRepository : ITicketInterface
    {

        public AirlineDbContext _AirlineDbContext;
        public TicketJsonRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }

        private string _path;
        public TicketJsonRepository(string path)
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
        public void book(Ticket newTicket)
        {
            try
            {
                newTicket.Id = Guid.NewGuid();

                var list = getTickets().ToList();
                list.Add(newTicket);

                var allSeatsText = JsonSerializer.Serialize(list);


                File.WriteAllText(_path, allSeatsText);

            }
            catch (Exception ex)
            {
                throw new Exception("Error during booking process!");
            }
        }

        public void cancelTicket(Guid chosenTicketId)
        {
            var list = getTickets().ToList();
            var chosenTicket = getTicket(chosenTicketId);
            var ticketToUpdate = list.FirstOrDefault(x => x.Id == chosenTicketId);

            if (ticketToUpdate != null)
            {
                if (ticketToUpdate.Cancelled == true)
                {
                    ticketToUpdate.Cancelled = false;
                }
                else
                {
                    ticketToUpdate.Cancelled = true;
                }

                try
                {
                    var allFlightsText = JsonSerializer.Serialize(list);
                    File.WriteAllText(_path, allFlightsText);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating a ticket", ex);
                }
            }
            else
            {
                throw new Exception($"Ticet with ID {chosenTicketId} not found.");
            }
        }

        public void DeleteTicket(Guid ticketID)
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
                        throw new Exception("Error while deleteing the Ticket");
                    }
                    else
                    {
                        var myTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);  //converts from Json string into an object (list of products)
                        var objectsToRemove = myTickets.Where(item => item.Id == ticketID);        //create the list of seats with the same id, should be only one
                        foreach (var objectToRemove in objectsToRemove)                       // go through all of them and delete them
                        {
                            myTickets.Remove(objectToRemove);
                        }
                        string updatedJson = JsonSerializer.Serialize(myTickets, new JsonSerializerOptions { WriteIndented = true });

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

        public Ticket? getTicket(Guid ticketIt)
        {
            return getTickets().SingleOrDefault(x => x.Id == ticketIt);
        }

        public IQueryable<Ticket> getTickets()
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
                        return new List<Ticket>().AsQueryable();

                    }

                    List<Ticket> mySeats = JsonSerializer.Deserialize<List<Ticket>>(allText);  //converts from Json string into an object (list of products)
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
                throw new Exception("File saving tickets not found");
            }
        }

        public void updateTicket(Ticket chosenTicket)
        {
            var list = getTickets().ToList();
            var chTicket = getTicket(chosenTicket.Id);
            var ticketToUpdate = list.FirstOrDefault(x => x.Id == chosenTicket.Id);

            if (ticketToUpdate != null)
            {
                if (ticketToUpdate.Cancelled == true)
                {
                    ticketToUpdate.SeatFk = chosenTicket.SeatFk;
                    ticketToUpdate.Seat = chosenTicket.Seat;
                    ticketToUpdate.Owner = chosenTicket.Owner;
                    ticketToUpdate.PassportImage = chosenTicket.PassportImage;
                    ticketToUpdate.PricePaid = ticketToUpdate.PricePaid;
                    ticketToUpdate.Cancelled = false;
                }

                try
                {
                    var allFlightsText = JsonSerializer.Serialize(list);
                    File.WriteAllText(_path, allFlightsText);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while updating a TIcket", ex);
                }
            }
            else
            {
                throw new Exception($"Flight with ID {chosenTicket} not found.");
            }
        }
    }
}
