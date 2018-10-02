using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public class ComputerController : Controller
    {
        private readonly IConfiguration _config;

        public ComputerController(IConfiguration config)
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
                c.ComputerId,
                c.DatePurchased,
                c.DateDecommissioned,
                c.Working,
                c.ModelName,
                c.Manufacturer
             
        
            from Computer c
    
        ";

            using (IDbConnection conn = Connection)
            {
                IEnumerable<Computer> computer = await conn.QueryAsync<Computer>(sql);

                return View(computer);

            }
        }
        public ActionResult Create(int id)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Computer computer)
        {

            if (ModelState.IsValid)
            {
                string sql = $@"
                    INSERT INTO Computer
                        ( DatePurchased, DateDecommissioned, Working, ModelName, Manufacturer )
                        VALUES
                        ( 
                             '{computer.DatePurchased}'
                            , '{computer.DateDecommissioned}'
                            , '{computer.Working}'
                            , '{computer.ModelName}'
                            , '{computer.Manufacturer}'
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

            return View(computer);
        }

        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string sql = $@"
                select
                c.ComputerId,   
                c.DatePurchased,
                c.DateDecommissioned,
                c.Working,
                c.ModelName,
                c.Manufacturer
                FROM Computer c
                WHERE c.ComputerId = {id}";

            using (IDbConnection conn = Connection)
            {
                Computer computer = await conn.QueryFirstAsync<Computer>(sql);

                if (computer == null) return NotFound();

                return View(computer);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if ( id == null)
            {
                return NotFound();
            }
            string sql = $@"DELETE FROM EmployeeComputer WHERE ComputerId = {id};
                            DELETE FROM Computer WHERE ComputerId = {id}";



            using (IDbConnection conn = Connection)
            {
                int rowsAffected = await conn.ExecuteAsync(sql);
                if (rowsAffected > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                throw new Exception("No rows affected");
            
            
            
            
            }
        }
               

        }
    }
