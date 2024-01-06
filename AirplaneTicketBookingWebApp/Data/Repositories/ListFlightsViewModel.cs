namespace Data.Repositories
{
    internal class ListFlightsViewModel
    {
        public ListFlightsViewModel()
        {
        }

        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double WholesalePrice { get; set; }
        public double CommissionRate { get; set; }
    }
}