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
    public class SeatDbRepository : ISeatRepository
    {
        public AirlineDbContext _AirlineDbContext;
        public SeatDbRepository(AirlineDbContext AirlineDbContext)
        {
            _AirlineDbContext = AirlineDbContext;
        }

        public void CreateSeat(Seat newSeat)
        {
            _AirlineDbContext.Seats.Add(newSeat);
            _AirlineDbContext.SaveChanges();

        }

        public Seat? GetSeat(Guid seatId)
        {
            return _AirlineDbContext.Seats.SingleOrDefault(x => x.Id == seatId);
        }

        public IQueryable<Seat> GetSeats()
        {
            return _AirlineDbContext.Seats;
        }

        public void DeleteSeat(Guid seatId)
        {
            var seatToDelete = GetSeat(seatId);
            if (seatToDelete != null)
            {
                _AirlineDbContext.Remove(seatToDelete);
                _AirlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No ticket to delete!");
            }
        }



    }
}
