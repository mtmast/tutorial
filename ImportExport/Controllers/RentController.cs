using ImportExport.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;

namespace ImportExport.Controllers
{
    public class RentController : Controller
    {
        public static string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        // GET: RentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: RentController/SingleFileUpload
        [HttpPost]
        public async Task<ActionResult> SingleFileUpload(IFormFile SingleFile)
        {
            if (ModelState.IsValid)
            {
                if (SingleFile != null && SingleFile.Length > 0) // File Payload has or not
                {
                    var fileExtension = Path.GetExtension(SingleFile?.FileName);
                    if (fileExtension != ".xlsx" && fileExtension != ".csv") // File Payload Validation 
                    {
                        ModelState.AddModelError("", "Please upload a file with .xlsx or .csv extension.");
                        return View("Index"); // Return to the view with the error message
                    }
                    else
                    {
                        if (!Directory.Exists(FolderPath))
                        {
                            Directory.CreateDirectory(FolderPath);
                        }

                        var FilePath = Path.Combine(FolderPath, SingleFile.FileName);
                        Debug.Write("Your File" + FilePath);
                        if (System.IO.File.Exists(FilePath))
                        {
                            TempData["MessageType"] = "fail";
                            TempData["Message"] = "Your File is Already Exist";
                            return View("Index"); // Return to the view with the error message
                        }
                        using (var streams = System.IO.File.Create(FilePath))
                        {
                            await SingleFile.CopyToAsync(streams);
                        }

                        // import to database 
                        var stream = SingleFile.OpenReadStream();
                        var customers = new List<TblCustomer>();
                        var renters = new List<TblRent>();
                        var movies = new List<TblMovie>();
                        int TemporaryId;
                        try
                        {
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    TemporaryId = Convert.ToInt32(DateTime.Now.Ticks % 100000000) ;
                                    try
                                    {

                                        string movieTitle = worksheet?.Cells[row, 3].Value?.ToString();
                                        customers.Add(
                                           new TblCustomer()
                                           {
                                               CusId = TemporaryId,
                                               FullName = worksheet.Cells[row, 1].Value?.ToString(),
                                               Salutation = worksheet.Cells[row, 4].Value?.ToString(),
                                               Address = worksheet.Cells[row, 2].Value?.ToString(),
                                               CreatedAt = DateTime.Now
                                           }
                                       );
                                        
                                        using (var context = new DBContext())
                                        {
                                          
                                            int movieId = await context.TblMovies
                                                            .Where(c => c.Title == movieTitle)
                                                            .Select(d => d.MvId) 
                                                            .FirstOrDefaultAsync();
                                            Debug.Write("TBLTBLTBLMOVIE-------=============" + movieId);

                                            if (movieId > 0)
                                            {
                                                renters.Add(
                                                    new TblRent()
                                                    {
                                                        RentId = TemporaryId,
                                                        CusId = TemporaryId,
                                                        MvId = movieId, 
                                                        RentAt = DateTime.Now
                                                    }
                                                );
                                                context.Add(renters);
                                                context.Add(customers);
                                                await context.SaveChangesAsync();
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.Write("Errrrrrrrrrrrrrrrrrrrrrr From Inner Try" + ex);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Write("Errrrrrrrrrrrrrrrrrrrrrr" + e);
                            return View("Index");
                        }

                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Select File");
                    return View("Index"); // Return to the view with the error message
                }
            }
            return View("Index");
        }

        // GET: RentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
