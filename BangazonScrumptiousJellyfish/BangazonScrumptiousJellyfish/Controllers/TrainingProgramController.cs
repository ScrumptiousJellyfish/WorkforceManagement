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
    public class TrainingProgramController : Controller
    {
        private readonly IConfiguration _config;

        public TrainingProgramController(IConfiguration config)
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

        // GET: TrainingProgram
        public async Task<IActionResult> Index()
        {
            string sql = @"select * from TrainingProgram 
                           where StartDate > (select CAST(GETDATE() as DATE))
                           ; ";
            using (IDbConnection conn = Connection)
            {
                List<TrainingProgram> programs = new List<TrainingProgram>();
                var programQuery = await conn.QueryAsync<TrainingProgram>(sql);
                programs = programQuery.ToList();
                return View(programs);
            }
        }

        // GET: TrainingProgram/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            string sql = $@"SELECT tp.*, e.*
                            FROM TrainingProgram tp
                            JOIN EmployeeTraining et ON et.TrainingProgramId = tp.TrainingProgramId
                            JOIN Employee e ON e.EmployeeId = et.EmployeeId
                            WHERE tp.TrainingProgramId = {id}";
            Dictionary<int, TrainingProgram> programs = new Dictionary<int, TrainingProgram>();
            using (IDbConnection conn = Connection)
            {
                TrainingProgram programInstance = new TrainingProgram();
                List<Employee> employeeList = new List<Employee>();
                var newQuery = await conn.QueryAsync<TrainingProgram, Employee, TrainingProgram>(sql,
                    (program, employee) =>
                    {
                        programInstance = program;
                        employeeList.Add(employee);

                        
                        return program;
                    },
                    splitOn: "employeeId"
                    );
                programInstance.Employees = employeeList;
                return View(programInstance);
            }
        }

        // GET: TrainingProgram/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingProgram/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind ("TrainingProgramId, ProgramName, Descrip, StartDate, EndDate, MaximumAttendees")] TrainingProgram trainingprogram)
        {
            if (ModelState.IsValid)
            {
                string sql = $@"
                    INSERT INTO TrainingProgram
                        (ProgramName, Descrip, StartDate, EndDate, MaximumAttendees)
                        VALUES
                        ('{trainingprogram.ProgramName}', '{trainingprogram.Descrip}', '{trainingprogram.StartDate}', '{trainingprogram.EndDate}', '{trainingprogram.MaximumAttendees}') 
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
            return View(trainingprogram);
        }

        // GET: TrainingProgram/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            string sql = $@"
                SELECT tp.*
                FROM TrainingProgram tp
                WHERE TrainingProgramId = {id}
            ";
            
            using (IDbConnection conn = Connection)
            {
                TrainingProgram instance = new TrainingProgram();
                instance = (await conn.QueryAsync<TrainingProgram>(sql)).Single();
                return View(instance);
            }
                
        }

        // POST: TrainingProgram/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, TrainingProgram program)
        {
            if(id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string sql = $@"
            UPDATE TrainingProgram
            SET ProgramName = '{program.ProgramName}',
                Descrip = '{program.Descrip}',
                StartDate = '{program.StartDate}',
                EndDate = '{program.EndDate}',
                MaximumAttendees = '{program.MaximumAttendees}'
                WHERE TrainingProgramId = {id}
            ";
                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);
                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    throw new Exception("No Rows Affected");
                }
            } else
            {
                return new StatusCodeResult(StatusCodes.Status406NotAcceptable);
            }

        }

        // GET: TrainingProgram/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingProgram/Delete/5
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