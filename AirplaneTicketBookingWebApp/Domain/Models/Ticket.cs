using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Id { get; set; }
        public int Row {  get; set; }
        public int Column { get; set; }
        [ForeignKey("Flight")]
        public Guid FlightFK { get; set; }       //Foreigh key

        public virtual Flight Flight { get; set; }  //Navigational
        public string Passport { get; set; }
        public double PricePaid { get; set; }
        public bool Cancelled { get; set; }



    }
}
