using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels.Tickets
{
    public class ListTicketsViewModel
    {
        public Guid Id { get; set; }
        [Required]
        
        public int Row { get; set; }
        public int Column { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightFK { get; set; }
        public string Passport { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }

    }
}
