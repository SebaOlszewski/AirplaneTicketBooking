using Data.DataContext;
using Domain.Models;
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

        public IQueryable<Seat> GetSeats()
        {
            return _AirlineDbContext.Seats;
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

    }
}
