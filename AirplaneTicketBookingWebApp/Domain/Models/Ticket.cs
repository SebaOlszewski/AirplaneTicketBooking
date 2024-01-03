using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Seat")]
        public Guid SeatFk { get; set; }       //Foreigh key

        public virtual Seat Seat { get; set; }  //Navigational

        public string PassportImage { get; set; } = string.Empty;

        public double PricePaid { get; set; }
        public bool Cancelled { get; set; } = false;
    }
}
