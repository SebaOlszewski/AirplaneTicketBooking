using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISeatRepository
    {
         void CreateSeat(Seat newSeat);
         Seat? GetSeat(Guid seatId);
         IQueryable<Seat> GetSeats();

         void DeleteSeat(Guid seatId);
        

    }
}
