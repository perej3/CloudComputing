using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Domain
{
    public class Booking
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }
        public string longitude  { get; set; }
        public string latitude { get; set; }
        public string longitudeTo { get; set; }
        public string latitudeTo { get; set; }
        public virtual IdentityUser Passenger { get; set; }

        [ForeignKey("CarCategory")]
        public int CategoryId { get; set; }
        public virtual CarCategory CarCategory { get; set; }



    }
}
