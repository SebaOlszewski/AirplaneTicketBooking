namespace Presentation.Models.ViewModels
{
    public class EditFlightViewModel
    {

        public Guid Id { get; set; }    //this is needed to know which flight to edit
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double CommissionRate { get; set; }
        public double WholesalePrice { get; set; }


    }
}
