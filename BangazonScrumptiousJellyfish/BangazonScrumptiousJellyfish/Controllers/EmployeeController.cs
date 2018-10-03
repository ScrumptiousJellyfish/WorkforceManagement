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
                string sql = $@"
                    INSERT INTO Employee
                        ( FirstName, LastName, Email, Supervisor, DepartmentId )
                        VALUES
                        ( null
                            , '{employee.FirstName}'
                            , '{employee.LastName}'
                            , '{employee.Email}'
                            , '{employee.Supervisor}'
                            , {employee.DepartmentId}
                        )
                    ";
                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
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
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sql = $@"
                SELECT
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    c.ComputerId,
                    c.ModelName,
                    d.DepartmentId,
                    d.DepartmentName,
                    ec.EmployeeComputerId,
                    ec.EmployeeId,
                    ec.ComputerId
                FROM Employee e
                JOIN Department d on d.DepartmentId = e.DepartmentId
                JOIN EmployeeComputer ec on e.EmployeeId = ec.EmployeeId
                JOIN Computer c on ec.ComputerId = c.ComputerId
                WHERE e.EmployeeId = {id}";

            using (IDbConnection conn = Connection)
            {
                EmployeeEditViewModel model = new EmployeeEditViewModel(_config);

                model.Employee = (await conn.QueryAsync<Employee, Computer, Department, Employee>(sql,
                    (employee, computer, department) =>
                    {
                        employee.Department = department;
                        employee.Computer = computer;
                        return employee;

                    }, splitOn: "ComputerId, DepartmentId"
                    )).Distinct().First();
                return View(model);
            }

        }
        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EmployeeEditViewModel model)
        {
            Console.WriteLine(id);
            if (id != model.Employee.EmployeeId)
            {
                
                return NotFound();
            }

            ModelState.Remove("Employee.Department.DepartmentName");
            ModelState.Remove("Employee.Department.ExpenseBudget");

            if (ModelState.IsValid)
            {
                string sql = $@"
                IF (OBJECT_ID('dbo.FK_Department', 'F') IS NOT NULL)
                BEGIN
                ALTER TABLE dbo.Employee DROP CONSTRAINT FK_Department
                END";

                string sql2 = $@"
                UPDATE Employee
                SET FirstName = '{model.Employee.FirstName}',
                    LastName = '{model.Employee.LastName}',
                    Email = '{model.Employee.Email}',
                    DepartmentId = '{model.Employee.Department.DepartmentId}'
                WHERE EmployeeId = {id}";

                //string sql3 = $@"
                //            UPDATE EmployeeComputer
                //    SET ComputerId = '{model.Employee.Computer.ComputerId}'
                //WHERE EmployeeId = {id}";

                string sql4 = $@"
                ALTER TABLE Employee
                ADD CONSTRAINT [FK_Department]
                FOREIGN KEY (DepartmentId) REFERENCES Department(DepartmentId)";

                using (IDbConnection conn = Connection)
                using (IDbConnection conn2 = Connection)
                using (IDbConnection conn4 = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    int rowsAffected2 = await conn2.ExecuteAsync(sql2);
                    int rowsAffected4 = await conn4.ExecuteAsync(sql4);
                    if (/*rowsAffected > 0 &&*/ rowsAffected2 > 0 /*&& rowsAffected4 > 0*/)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    throw new Exception("No rows affected");
                }
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
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