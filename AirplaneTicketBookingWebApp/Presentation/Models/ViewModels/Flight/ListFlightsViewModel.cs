using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Flight
{
    public class ListFlightsViewModel
    {
        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime TimeToDeparture {  get; set; }

        public string CountryFrom { get; set; } = string.Empty;
        public string CountryTo { get; set; } = string.Empty;

        public List<Seat> seatingList { get; set; }

        public double WholesalePrice { get; set; }

        public double CommissionRate { get; set; }

        
    }
}
