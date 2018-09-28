using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonScrumptiousJellyfish.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonScrumptiousJellyfish.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            string sql = $@"select * from Employee";
            using (IDbConnection conn = Connection)
            {
                List<Employee> employees = (await conn.QueryAsync<Employee>(sql)).ToList();
                return View(employees);
            }
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sql = $@"
            select 
            e.FirstName, e.LastName, d.DepartmentName, c.ModelName, tp.ProgramName
            from Employee e 
            join Department d on e.DepartmentId = d.DepartmentId
            join EmployeeComputer ec on ec.EmployeeId = e.EmployeeId
            join Computer c on ec.ComputerId = c.ComputerId
            join EmployeeTraining et on et.EmployeeId = e.EmployeeId
            join TrainingProgram tp on et.TrainingProgramId = tp.TrainingProgramId
            where e.EmployeeId = {id}";

            using (IDbConnection conn = Connection)
            {

                Employee employee = (await conn.QueryAsync<Employee>(sql)).ToList().Single();

                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}