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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public ActionResult Index()
            {
                return View();
            }

            // GET: Employee/Details/5
            public ActionResult Details(int id)
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
