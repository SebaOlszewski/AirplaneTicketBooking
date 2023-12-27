using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Repositories;
using Domain.Models;

namespace Presentation.Models.ViewModels
{
    public class CreateFlightViewModel
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public string Owner { get; set; }



    }
}
