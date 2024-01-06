using Data.DataContext;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SeatDbRepository
    {

        public AirlineDbContext _AirlineDbContext;
        public SeatDbRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }

        public void AddSeat(Seat newSeat)
        {
            _AirlineDbContext.Seats.Add(newSeat);
            _AirlineDbContext.SaveChanges();
        }


        

        public Seat? GetSeat(Guid seatId)
        {
            return GetSeats().SingleOrDefault(x => x.Id == seatId);
        }

        public IQueryable<Seat> GetAllTheSeatsFromAFlight(Guid flightId)
        {
            return _AirlineDbContext.Seats.Where(x => x.FlightFk == flightId);
        }

        public IQueryable<Seat> GetSeats()
        {
            return _AirlineDbContext.Seats;
        }
        
        public int getMaxRowsFromAFlight(Guid flightID)
        {
            var seatsInFlight = GetAllTheSeatsFromAFlight(flightID);
            int maxRow = seatsInFlight.Max(x => x.Row);
            return maxRow;
        }

        public int getMaxColumnsFromAFlight(Guid flightID)
        {
            var seatsInFlight = GetAllTheSeatsFromAFlight(flightID);
            int maxColumn = seatsInFlight.Max(x => x.Column);
            return maxColumn;
        }

        


        public void deleteSeat(Guid seatId)
        {
            var seaetToDelete = GetSeat(seatId);
            if (seaetToDelete != null)
            {
                _AirlineDbContext.Remove(seaetToDelete);
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No seat to delete");
            }
        }

        public void deleteChosenSeatInAFlight(Guid flightId, int col, int row)
        {
            var seatToDelete = GetSeats().Any(x => x.FlightFk == flightId && x.Column == col && x.Row == row);
            
            if (seatToDelete != null)
            {
                
                _AirlineDbContext.Remove(seatToDelete);
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No seat to delete");
            }
        }




        public void updateSeat(Seat chosenSeat)
        {
            var seatToUpdate = GetSeat(chosenSeat.Id);
            if (seatToUpdate != null)
            {
                seatToUpdate.Row = chosenSeat.Row;
                seatToUpdate.Column = chosenSeat.Column;
                seatToUpdate.FlightFk = chosenSeat.FlightFk;
                seatToUpdate.isTaken = chosenSeat.isTaken;
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No seat to update");
            }
        }

        public bool isSeatTaken(Guid seatId)
        {
            var seatToUpdate = GetSeat(seatId);

            if(seatToUpdate.isTaken == true)
            {
                return true;
            }
            return false;
            
        }

        public void takeSeat(Guid seatId)
        {
            var seatToUpdate = GetSeat(seatId);
            if (seatToUpdate != null && seatToUpdate.isTaken == false)
            {
                seatToUpdate.isTaken = true;
                _AirlineDbContext.SaveChanges();
            }
            else if(seatToUpdate != null && seatToUpdate.isTaken == true)
            {
                seatToUpdate.isTaken = false;
                _AirlineDbContext.SaveChanges();
            }else
            {
                throw new Exception("No seed to take/free");
            }
        }



    }
}
