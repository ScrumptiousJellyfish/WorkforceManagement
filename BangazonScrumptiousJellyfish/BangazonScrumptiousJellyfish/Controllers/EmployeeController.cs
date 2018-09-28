﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                    }, splitOn:"DepartmentId");
                return View(employees.Values);
            }
        }

        // GET: Employee
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}