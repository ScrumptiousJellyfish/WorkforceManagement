using System;
using System.Collections.Generic;

namespace Workforce.Models
{
    public class EmployeeComputer
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ComputerId { get; set; }
        public DateTime DateAssigned { get; set; }
        public DateTime DateReturned { get; set; }
    }

}