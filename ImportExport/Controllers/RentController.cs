using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                            ModelState.AddModelError("", "Your File is Already Exist");
                            return View("Index"); // Return to the view with the error message
                        }
                        using (var stream = System.IO.File.Create(FilePath))
                        {
                            await SingleFile.CopyToAsync(stream);
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
