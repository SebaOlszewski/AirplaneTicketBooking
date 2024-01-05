using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Flight
{
    public class CreateFlightViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }

        public DateTime DepartureDate { get; set; } = new DateTime(2000, 1, 1, 0, 0, 0);
        public DateTime ArrivalDate { get; set; } = new DateTime(2000, 1, 1, 0, 0, 0);

        public string CountryFrom { get; set; } = string.Empty;
        public string CountryTo { get; set; } = string.Empty;

        public double WholesalePrice { get; set; }

        public double CommissionRate { get; set; }

    }
}
