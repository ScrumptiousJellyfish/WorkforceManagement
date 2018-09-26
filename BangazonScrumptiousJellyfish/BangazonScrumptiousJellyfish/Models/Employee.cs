using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonScrumptiousJellyfish.Models
{
    public class Employee
    {
        
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public bool IsSupervisor { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Display(Name = "Employee Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public Computer Computer { get; set; }
    }
}
