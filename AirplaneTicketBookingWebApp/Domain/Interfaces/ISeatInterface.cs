using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISeatInterface
    {
        void AddSeat(Seat newSeat);
        Seat? GetSeat(Guid seatId);
        IQueryable<Seat> GetAllTheSeatsFromAFlight(Guid flightId);
        IQueryable<Seat> GetSeats();

        int getMaxRowsFromAFlight(Guid flightID);

        int getMaxColumnsFromAFlight(Guid flightID);

        void deleteSeat(Guid seatId);

        void deleteChosenSeatInAFlight(Guid flightId, int col, int row);
        void updateSeat(Seat chosenSeat);
        bool isSeatTaken(Guid seatId);
        void takeSeat(Guid seatId);
        
    }
}
