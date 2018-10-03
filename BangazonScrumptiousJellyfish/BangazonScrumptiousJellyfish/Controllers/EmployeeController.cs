using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BangazonScrumptiousJellyfish.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
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
        public async Task<IActionResult> Index()
        {
            string sql = @"
            select
                e.EmployeeId,
                e.FirstName,
                e.LastName,
                e.Email,
                e.Supervisor,
                d.DepartmentId,
                d.DepartmentName
            FROM Employee e
            join Department d on e.DepartmentId = d.DepartmentId";
            using (IDbConnection conn = Connection)
            {
                Dictionary<int, Employee> employees = new Dictionary<int, Employee>();
                var employeeQuerySet = await conn.QueryAsync<Employee, Department, Employee>(
                    sql,
                    (employee, department) =>
                    {
                        if (!employees.ContainsKey(employee.EmployeeId))
                        {
                            employees[employee.EmployeeId] = employee;
                        }
                        employees[employee.EmployeeId].Department = department;
                        return employee;
                    }, splitOn: "DepartmentId");
                return View(employees.Values);
            }
        }
        // GET: Employee/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sql = $@"
            select 
            e.EmployeeId, e.FirstName, e.LastName, d.DepartmentId, d.DepartmentName, c.ComputerId, c.ModelName, tp.TrainingProgramId, tp.ProgramName
            from Employee e 
            join Department d on e.DepartmentId = d.DepartmentId
            join EmployeeComputer ec on ec.EmployeeId = e.EmployeeId
            join Computer c on ec.ComputerId = c.ComputerId
            join EmployeeTraining et on et.EmployeeId = e.EmployeeId
            join TrainingProgram tp on et.TrainingProgramId = tp.TrainingProgramId
            where e.EmployeeId = {id}";

            using (IDbConnection conn = Connection)
            {
                Dictionary<int, Employee> employeeDictionary = new Dictionary<int, Employee>();

                var list = conn.Query<Employee, Department, Computer, TrainingProgram, Employee>(sql, (employ, department, computer, trainingProgram) =>
                {
                    Employee emp;
                    if (!employeeDictionary.TryGetValue(employ.EmployeeId, out emp))
                    {
                        emp = employ;
                        emp.Department = department;
                        emp.Computer = computer;
                        emp.TrainingPrograms = new List<TrainingProgram>();
                        employeeDictionary.Add(emp.EmployeeId, emp);
                    }

                    emp.TrainingPrograms.Add(trainingProgram);
                    return emp;
                }, splitOn: "EmployeeId,DepartmentId,ComputerId,TrainingProgramId").Distinct().First();


                return View(list);
            }
        }
        // GET: Employee/Create
        public ActionResult Create(int id)
        {
            return View();
        }
        // GET: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {

    //            string sql2 = $@"
				//IF(OBJECT_ID('dbo.FK_Department','F') IS NOT NULL)
    //            BEGIN
    //            ALTER TABLE dbo.Employee
    //            DROP Constraint FK_Department
    //            END";
                

                string sql = $@"
                   
            
                    INSERT INTO Employee
                        ( FirstName, LastName, Email, Supervisor, DepartmentId )
                        VALUES
                        (  '{employee.FirstName}'
                            , '{employee.LastName}'
                            , '{employee.Email}'
                            , '{employee.Supervisor}'
                            ,'{employee.DepartmentId}'
                        )
                    ";
    //            string sql3 = $@"
    //            ALTER TABLE dbo.Employee
				//ADD constraint [FK_Department]
				//FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId)";

                using (IDbConnection conn = Connection)
                {
                    //int rowsAffected = await conn.ExecuteAsync(sql2);
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    //int rowsAffected3 = await conn.ExecuteAsync(sql3);

                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(employee);
        }
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string sql = $@"
                select
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Supervisor,
                    e.DepartmentId,
                    e.Manufacturer
                FROM employee e
                WHERE e.EmployeeId = {id}";
            using (IDbConnection conn = Connection)
            {
                Employee employee = (await conn.QueryAsync<Employee>(sql)).ToList().Single();
                if (employee == null)
                {
                    return BadRequest();
                }
                return View(employee);
            }
        }
      
        //// POST: Employee/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //// GET: Employee/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}