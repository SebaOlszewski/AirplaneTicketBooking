using Data.DataContext;
using Domain.Interfaces;
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
        public SeatDbRepository _seatRepository;
        public FlightDbRepository(AirlineDbContext AirlineDbContext, SeatDbRepository seatRepository)
        {
            _AirlineDbContext = AirlineDbContext;
            _seatRepository = seatRepository;

        }

        public void AddFlight(Flight newFlight)
        {
            _AirlineDbContext.Flights.Add(newFlight);


            //Addings feat after creating the flight
            int maxRows = newFlight.Rows;
            int maxColumns = newFlight.Columns;
            Guid flightID = newFlight.Id;
            //_AirlineDbContext.SaveChanges();
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    _seatRepository.AddSeat(new Seat()
                    {
                        Row = row,
                        Column = col,
                        FlightFk = flightID,
                        isTaken = false,
                    });
                }
            }

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
                var seatsToDelete = _seatRepository.GetAllTheSeatsFromAFlight(FlightId).ToList();
                foreach (var seat in seatsToDelete)
                {
                    _seatRepository.deleteSeat(seat.Id);
                }


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
            //old data         //new data
            var flightToUpdate = GetFlight(chosenFlight.Id);
            if (flightToUpdate != null)
            {

                int oldRow = flightToUpdate.Rows;
                int oldColumn = flightToUpdate.Columns;
                int newRow = chosenFlight.Rows;
                int newColumn = chosenFlight.Columns;


                //expanding the seat database by the difference in the seats


                //expanding in both/onedirection
                if (newRow >= oldRow && newColumn >= oldColumn)
                {
                    for (int row = 0; row < newRow; row++)
                    {
                        for (int col = 0; col < newColumn; col++)
                        {
                            if(row >= oldRow || col >= oldColumn)
                            {
                                _seatRepository.AddSeat(new Seat()
                                {
                                    Row = row,
                                    Column = col,
                                    FlightFk = chosenFlight.Id,
                                    isTaken = false,
                                });
                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }

                //decreasing in both directions
                if (newRow <= oldRow && newColumn <= oldColumn)
                {
                    for (int row = 0; row < oldRow; row++)
                    {
                        for (int col = 0; col < oldColumn; col++)
                        {
                            if (row >=newRow || col >= newColumn)
                            {
                                _seatRepository.deleteChosenSeatInAFlight(chosenFlight.Id, row, col);
                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }

                //increasing rows, decreasing columns
                if (newRow >= oldRow && newColumn <= oldColumn)
                {
                    for (int row = 0; row < newRow; row++)
                    {
                        for (int col = 0; col < newColumn; col++)
                        {
                            if (row <= newRow && col >= oldColumn)
                            {
                                _seatRepository.deleteChosenSeatInAFlight(chosenFlight.Id, row, col);
                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }

                //decreasing rows, increasing  columns
                if (newRow <= oldRow && newColumn >= oldColumn)
                {
                    for (int row = 0; row < oldRow; row++)
                    {
                        for (int col = 0; col < newColumn; col++)
                        {
                            
                            if (row <= newRow -1 && col >= oldColumn && col <= newColumn)
                            {
                                _seatRepository.AddSeat(new Seat()
                                {
                                    Row = row,
                                    Column = col,
                                    FlightFk = chosenFlight.Id,
                                    isTaken = false,
                                });
                            }
                            else if (row >= newRow && col <= oldColumn - 1)   //-1 cause we start counting from 0 not 1
                            {
                                _seatRepository.deleteChosenSeatInAFlight(chosenFlight.Id, col, row);
                            }
                            
                            
                        }
                    }
                }

                //increasing rows, decreasing  columns
                if (newRow >= oldRow && newColumn <= oldColumn)
                {
                    for (int row = 0; row < newRow; row++)
                    {
                        for (int col = 0; col < oldColumn; col++)
                        {
                            
                            if (row <= oldRow - 1 && col < oldColumn && col >= newColumn)
                            {
                                _seatRepository.deleteChosenSeatInAFlight(chosenFlight.Id, col, row);
                            }
                            else if (row >= oldRow && col <= newColumn - 1)   //-1 cause we start counting from 0 not 1
                            {
                                _seatRepository.AddSeat(new Seat()
                                {
                                    Row = row,
                                    Column = col,
                                    FlightFk = chosenFlight.Id,
                                    isTaken = false,
                                });
                            }
                            
                        }
                    }
                }



                flightToUpdate.Rows = newRow;
                flightToUpdate.Columns = newColumn;
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
