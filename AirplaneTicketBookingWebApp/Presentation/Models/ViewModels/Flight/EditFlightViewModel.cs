namespace Presentation.Models.ViewModels.Flight
{
    public class EditFlightViewModel
    {
        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; } = string.Empty;
        public string CountryTo { get; set; } = string.Empty;
        public double WholesalePrice { get; set; }
        public double CommissionRate { get; set; }
    }
}
