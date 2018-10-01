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
        public string ProgramName { get; set; }

        [Required]
        [Display (Name = "Description")]
        public string Descrip { get; set; }

        [Required]
        [Display (Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display (Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public int MaximumAttendees { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
