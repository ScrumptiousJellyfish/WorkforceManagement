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
    public class DepartmentController : Controller
    {

        private readonly IConfiguration _config;

        public DepartmentController(IConfiguration config)
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


        // GET: Department
        public async Task<IActionResult> Index()
        {
            string sql = @"
            SELECT
                d.DepartmentId,
                d.DepartmentName,
                d.ExpenseBudget
            FROM Department d;
        ";

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Department> department = await conn.QueryAsync<Department>(sql);

                return View(department);

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
                d.DepartmentName,
                d.ExpenseBudget
            from Department d
            WHERE d.DepartmentId = {id}";

            using (IDbConnection conn = Connection)
            {

                Department department = (await conn.QueryAsync<Department>(sql)).ToList().Single();

                if (department == null)
                {
                    return NotFound();
                }

                return View(department);
            }
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind("DepartmentId, DepartmentName, ExpenseBudget")] Department department)
        {

            if (ModelState.IsValid)
            {
                string sql = $@"
                    INSERT INTO Department
                        (DepartmentName, ExpenseBudget)
                        VALUES
                        ('{department.DepartmentName}', '{department.ExpenseBudget}') 
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
            return View (department);
        }
           
        

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Department/Edit/5
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

        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Department/Delete/5
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