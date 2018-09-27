using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonScrumptiousJellyfish.Models
{
    public class EmployeeTraining
    {
        public int EmployeeTrainingId { get; set; }
        public int EmployeeId { get; set; }
        public int TrainingProgramId { get; set; }
    }
}
