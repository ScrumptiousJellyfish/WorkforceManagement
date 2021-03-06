﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonScrumptiousJellyfish.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Required]
        [Display(Name = "Expense Budget")]
        public int ExpenseBudget { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
