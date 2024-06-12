using Microsoft.AspNetCore.Mvc;
using SalaryPortal.Data;
using SalaryPortal.Models.Entities;
using SalaryPortal.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System.Web;


namespace SalaryPortal.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public SalaryController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new EmployeeViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(EmployeeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var salaries = new Models.Entities.Salary
                    {
                        Gross_Salary = viewModel.Gross_Salary,
                        Basic_Salary = viewModel.Basic_Salary,
                        HRA = viewModel.HRA,
                        Medical_Allowance = viewModel.Medical_Allowance,
                        Employee_Id = viewModel.Employee_Id,
                    };


                    dbContext.Salary.Add(salaries);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(List));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error occurred while saving data: " + ex.Message);
                    return View(viewModel);
                }

            }
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult List(int? page, string sortOrder, string currentFilter, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["GrossSalarySortParam"] = string.IsNullOrEmpty(sortOrder) ? "grossSalary_desc" : "";
            ViewData["EmployeeIdSortParam"] = sortOrder == "EmployeeId" ? "employeeId_desc" : "EmployeeId";

            if (searchString != null)
            {
                page = 1;
                sortOrder = "";
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var salaries = from s in dbContext.Salary
                           select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                salaries = salaries.Where(s => s.Employee_Id.ToString().Contains(searchString) ||
                                               s.Gross_Salary.ToString().Contains(searchString) ||
                                               s.Basic_Salary.ToString().Contains(searchString) ||
                                               s.Medical_Allowance.ToString().Contains(searchString) ||
                                               s.HRA.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "grossSalary_desc":
                    salaries = salaries.OrderByDescending(s => s.Gross_Salary);
                    break;
                case "EmployeeId":
                    salaries = salaries.OrderBy(s => s.Employee_Id);
                    break;
                case "employeeId_desc":
                    salaries = salaries.OrderByDescending(s => s.Employee_Id);
                    break;
                default:
                    salaries = salaries.OrderBy(s => s.Gross_Salary);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // Calculate the total number of pages
            int totalSalaries = salaries.Count();
            int totalPages = (int)Math.Ceiling((double)totalSalaries / pageSize);

            // Ensure the pageNumber is within valid range
            pageNumber = Math.Max(1, Math.Min(totalPages, pageNumber));
            var pagedSalaries = salaries.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            //var convertedModel = new StaticPagedList<Salary>(pagedSalaries, pageNumber, pageSize, totalSalaries);
            var convertedModel = new X.PagedList.StaticPagedList<SalaryPortal.Models.Entities.Salary>(pagedSalaries, pageNumber, pageSize, salaries.Count());
            return View(convertedModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var salary = dbContext.Salary.Find(id);

            if (salary == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeViewModel
            {
                Salary_Id = salary.Salary_Id,
                Gross_Salary = salary.Gross_Salary,
                Basic_Salary = salary.Basic_Salary,
                Medical_Allowance = salary.Medical_Allowance,
                HRA = salary.HRA,
                Employee_Id = salary.Employee_Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Salary_Id, EmployeeViewModel viewModel)
        {
            if (Salary_Id != viewModel.Salary_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var salary = dbContext.Salary.Find(Salary_Id);
                    if (salary == null)
                    {
                        return NotFound();
                    }

                    salary.Gross_Salary = viewModel.Gross_Salary;
                    salary.Basic_Salary = viewModel.Basic_Salary;
                    salary.Medical_Allowance = viewModel.Medical_Allowance;
                    salary.HRA = viewModel.HRA;
                    //salary.Employee_Id = viewModel.Employee_Id;

                    dbContext.Update(salary);
                    dbContext.SaveChanges();

                    return RedirectToAction(nameof(List)); // assuming you have a List action in SalaryController
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return RedirectToAction(nameof(Edit), new { id = viewModel.Salary_Id });
                }
            }
            else
            {
                // Model is not valid, return the view with validation errors
                return View(viewModel);
            }
        }

        private bool SalaryExists(int id)
        {
            return dbContext.Salary.Any(e => e.Salary_Id == id);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salary = dbContext.Salary.Find(id);

            if (salary == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeViewModel
            {
                Salary_Id = salary.Salary_Id,
                Gross_Salary = salary.Gross_Salary,
                Basic_Salary = salary.Basic_Salary,
                Medical_Allowance = salary.Medical_Allowance,
                HRA = salary.HRA,
                Employee_Id = salary.Employee_Id
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int Salary_Id)
        {
            
            var salary = dbContext.Salary.Find(Salary_Id);

           
            if (salary == null)
            {
                return NotFound();
            }

            try
            {
                
                dbContext.Salary.Remove(salary);
                dbContext.SaveChanges();

               
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                
                return RedirectToAction(nameof(List)); 
            }
        }
        private bool SalaryExist(int id)
        {
            return dbContext.Salary.Any(e => e.Salary_Id == id);
        }

    }
} 


