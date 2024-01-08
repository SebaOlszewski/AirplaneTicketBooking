using Presentation.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Flight
{
    public class CreateFlightViewModel
    {
        [Required(ErrorMessage = "Please enter number of rows")]
        [Range(1, double.MaxValue, ErrorMessage = "Value out of range")]
        public int Rows { get; set; } = 1;
        [Required(ErrorMessage = "Please enter number of columns")]
        [Range(1, double.MaxValue, ErrorMessage = "Value out of range")]
        public int Columns { get; set; } = 1;

        [Date(ErrorMessage = "Invalid departure date")]
        public DateTime DepartureDate { get; set; } = new DateTime(2000, 1, 1, 0, 0, 0);
        [Date(ErrorMessage = "Invalid arrival date")]
        public DateTime ArrivalDate { get; set; } = new DateTime(2000, 1, 1, 0, 0, 0);

        public string CountryFrom { get; set; } = string.Empty;
        public string CountryTo { get; set; } = string.Empty;

        public double WholesalePrice { get; set; }

        public double CommissionRate { get; set; }

    }
}
