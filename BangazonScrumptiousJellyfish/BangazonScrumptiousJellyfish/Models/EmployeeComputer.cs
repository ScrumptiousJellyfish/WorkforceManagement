using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonScrumptiousJellyfish.Models
{
    public class EmployeeComputer
    {
        public int EmployeeComputerId { get; set; }

        [Required]
        [Display(Name = "Date Assigned")]
        public DateTime DateAssigned { get; set; }

        [Required]
        [Display(Name = "Date Returned")]
        public DateTime DateReturned { get; set; }

        public int EmployeeId { get; set; }

        public int ComputerId { get; set; }
    }
}
