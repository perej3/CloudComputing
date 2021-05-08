using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Domain
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int RegistrationPlate { get; set; }
        
        public int Capacity { get; set; }
        public string Condition{ get; set; }
        public bool Wifi { get; set; }
        public bool Aircondition{ get; set; }
        public bool FoodDrinks { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }

        [ForeignKey("CarCategory")]
        public int CategoryId  { get; set; }
        public virtual CarCategory CarCategory{ get; set; }
        
        [ForeignKey("IdentityUser")]

        public string DriverId { get; set; }
        public virtual IdentityUser Driver { get; set; }

    }
}
