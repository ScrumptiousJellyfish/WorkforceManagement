using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonScrumptiousJellyfish.Models
{
    public class TrainingProgram
    {

        public int TrainingProgramId { get; set; }
        [Required]
        [Display (Name = "Program Name")]
        public string Name { get; set; }
        [Required]
        [Display (Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display (Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        public int MaxAttendees { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
