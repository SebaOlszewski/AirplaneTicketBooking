using Data.Repositories;
using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Admin
{
    public class AdminListFlightsViewModel
    {
        public Guid Id { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        [Display(Name = "Departure Date: ")]
        public DateTime DepartureDate { get; set; }
        [Display(Name = "Arrival Date: ")]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "Departure from: ")]
        public string CountryFrom { get; set; } = string.Empty;

        [Display(Name = "Arrival to: ")]
        public string CountryTo { get; set; } = string.Empty;

        public List<Seat> seatingList { get; set; }

    }
}
