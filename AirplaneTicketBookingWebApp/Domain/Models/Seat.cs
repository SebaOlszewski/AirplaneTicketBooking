﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]

        public Guid Id { get; set; }
        public string? Owner { get; set; }
        

        [ForeignKey("Flight")]
        public Guid FlightFK { get; set; }       //Foreigh key

        public virtual Flight Flight { get; set; }  //Navigational

        public bool Taken { get; set; } = false;

        public string RowCol { get { return Row + "|" + Column; } }

        public int Row { get; set; }
        public int Column { get; set; }

    }
}
