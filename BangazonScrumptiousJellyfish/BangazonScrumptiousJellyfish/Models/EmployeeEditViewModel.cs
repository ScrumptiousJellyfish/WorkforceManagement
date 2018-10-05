using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;


namespace BangazonScrumptiousJellyfish.Models
{
    public class EmployeeEditViewModel
    {
        public Employee Employee { get; set; } = new Employee();

        [Display(Name = "Current Computer")]
        public List<SelectListItem> Computer { get; }

        [Display(Name = "Current Department")]
        public List<SelectListItem> Department { get; }

        public IEnumerable<string> TPCodes { get; set; }

        public List<SelectListItem> TrainingProgram { get; } = new List<SelectListItem>
        {
            //new SelectListItem { Value = "MX", Text = "Mexico" },
            //new SelectListItem { Value = "CA", Text = "Canada" },
            //new SelectListItem { Value = "US", Text = "USA"    },
            //new SelectListItem { Value = "FR", Text = "France" },
            //new SelectListItem { Value = "ES", Text = "Spain"  },
            //new SelectListItem { Value = "DE", Text = "Germany"}
         };


        private readonly IConfiguration _config;

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public EmployeeEditViewModel() { }

        public EmployeeEditViewModel(IConfiguration config)
        {
            _config = config;

            string sql = $@"SELECT ComputerId, ModelName FROM Computer";

            string sql2 = $@"SELECT DepartmentId, DepartmentName FROM Department";

            string sql3 = $@"SELECT TrainingProgramId, ProgramName FROM TrainingProgram";

            using (IDbConnection conn = Connection)
            using (IDbConnection conn2 = Connection)
            using (IDbConnection conn3 = Connection)
            {
                List<Computer> computer = (conn.Query<Computer>(sql)).ToList();

                this.Computer = computer
                    .Select(li => new SelectListItem
                    {
                        Text = li.ModelName,
                        Value = li.ComputerId.ToString()
                    }).ToList();

                List<Department> department = (conn2.Query<Department>(sql2)).ToList();

                this.Department = department
                    .Select(li => new SelectListItem
                    {
                        Text = li.DepartmentName,
                        Value = li.DepartmentId.ToString()
                    }).ToList();

                List<TrainingProgram> trainingPrograms = (conn3.Query<TrainingProgram>(sql3)).ToList();

                this.TrainingProgram = trainingPrograms
                    .Select(li => new SelectListItem
                    {
                        Text = li.ProgramName,
                        Value = li.TrainingProgramId.ToString()
                    }).ToList();
            }
            this.Computer.Insert(0, new SelectListItem
            {
                Text = "Choose Computer...",
                Value = "0"
            });

            this.Department.Insert(0, new SelectListItem
            {
                Text = "Choose Department...",
                Value = "0"
            });

            this.TrainingProgram.Insert(0, new SelectListItem
            {
                Text = "Choose Training Programs...",
                Value = "0"
            });
        }

        

       
    }
}
