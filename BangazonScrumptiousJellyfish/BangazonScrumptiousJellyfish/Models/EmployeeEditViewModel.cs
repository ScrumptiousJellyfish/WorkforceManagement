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
        public Employee Employee { get; set; }
        
        [Display(Name = "Current Computer")]
        public List<SelectListItem> Computer { get; }

        [Display(Name = "Current Department")]
        public List<SelectListItem> Department { get; }

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

            using (IDbConnection conn = Connection)
            {
                List<Computer> computer = (conn.Query<Computer>(sql)).ToList();

                this.Computer = computer
                    .Select(li => new SelectListItem
                    {
                        Text = li.ModelName,
                        Value = li.ComputerId.ToString()
                    }).ToList();
            }
            this.Computer.Insert(0, new SelectListItem
            {
                Text = "Choose Computer...",
                Value = "0"
            });

            string sql2 = $@"SELECT DepartmentId, DepartmentName FROM Department";

            using (IDbConnection conn = Connection)
            {
                List<Department> department = (conn.Query<Department>(sql2)).ToList();

                this.Department = department
                    .Select(li => new SelectListItem
                    {
                        Text = li.DepartmentName,
                        Value = li.DepartmentId.ToString()
                    }).ToList();
            }
            this.Department.Insert(0, new SelectListItem
            {
                Text = "Choose Department...",
                Value = "0"
            });
        }

        

       
    }
}
