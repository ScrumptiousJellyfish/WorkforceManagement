using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonScrumptiousJellyfish.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }

        [Required]
        [Display(Name = "Date Purchased")]
        public DateTime DatePurchased { get; set; }

        [Required]
        [Display(Name = "Date Commissioned")]
        public DateTime DateCommissioned { get; set; }

        [Required]
        public bool Working { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Required]
        public string Manufacturer { get; set; }
    }
}
