using ImportExport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;

namespace ImportExport.Controllers
{
    public class MessageService
    {
        public string? MessageType { get; set; }
        public string? Message { get; set; }
    }
    public class RentController : Controller
    {
        public static string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        // GET: RentController
        public async Task<ActionResult> Index()
        {
            List<VwCusMvRent> vwCusMvRent = new List<VwCusMvRent>();
            using(var context = new DBContext())
            {
                vwCusMvRent =await context.VwCusMvRents.ToListAsync();
            }
            return View(vwCusMvRent);
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
                    var fileExtension = Path.GetExtension(SingleFile.FileName);
                    if (fileExtension != ".xlsx" && fileExtension != ".csv") // File Payload Validation 
                    {
                        ModelState.AddModelError("", "Please upload a file with .xlsx or .csv extension.");
                        return RedirectToAction(nameof(Index)); // Return to the view with the error message
                    }
                    else
                    {
                        if (!Directory.Exists(FolderPath))
                        {
                            Directory.CreateDirectory(FolderPath);
                        }

                        var FilePath = Path.Combine(FolderPath, SingleFile.FileName);
                        if (System.IO.File.Exists(FilePath))
                        {
                            TempData["MessageType"] = "fail";
                            TempData["Message"] = "Your File is Already Exist";
                            return RedirectToAction(nameof(Index)); // Return to the view with the error message
                        }
                        using (var streams = System.IO.File.Create(FilePath))
                        {
                            await SingleFile.CopyToAsync(streams);
                        }

                        // import to database from xlsx or csv
                        var stream = SingleFile.OpenReadStream();
                        MessageService messages = new MessageService();
                        int TemporaryId = Convert.ToInt32(DateTime.Now.Ticks % 100000000);
                        try { 
                            switch (fileExtension) 
                            {
                                case ".xlsx": messages = await XlsxProcess(stream, TemporaryId);break;
                                case ".csv" : messages = await CsvProcess(stream, TemporaryId); break;
                                default: break;
                            }
                            TempData["MessageType"] = messages.MessageType;
                            TempData["Message"] = messages.Message;
                            return RedirectToAction(nameof(Index));

                        }
                        catch (Exception e)
                        {
                            messages = new MessageService
                            {
                                Message = "fail",
                                MessageType = "Something went wrong",
                            };
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Select File");
                    return RedirectToAction(nameof(Index)); // Return to the view with the error message
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // XLSX Processing Action and Return Temp Data Message for Condition
        public static async Task<MessageService> XlsxProcess(Stream stream, int TemporaryId)
        {
            MessageService result = new MessageService();
            TblCustomer? customers;
            TblRent? renters;
            int FalseDataCount = 0;
            int realDataCount = 0;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;

                for (var row = 2; row <= rowCount; row++)
                {
                    TemporaryId = TemporaryId + row;
                    realDataCount++;
                    try
                    {
                        string? movieTitle = worksheet?.Cells[row, 3].Value?.ToString() ??  string.Empty;
                        customers = new TblCustomer
                        {
                            CusId = TemporaryId,
                            FullName = worksheet?.Cells[row, 1].Value?.ToString(),
                            Salutation = worksheet?.Cells[row, 4].Value?.ToString(),
                            Address = worksheet?.Cells[row, 2].Value?.ToString(),
                            CreatedAt = DateTime.Now
                        };
                        using (var context = new DBContext())
                        {
                            int movieId = await context.TblMovies
                                            .Where(c => c.Title == movieTitle)
                                            .Select(d => d.MvId)
                                            .FirstOrDefaultAsync();
                            if (movieId > 0)
                            {
                                renters = new TblRent
                                {
                                    RentId = TemporaryId,
                                    CusId = TemporaryId,
                                    MvId = movieId,
                                    RentAt = DateTime.Now
                                };

                                context.Add(customers);
                                context.Add(renters);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                FalseDataCount++;
                            }
                          
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new MessageService
                        {
                            MessageType = "fail",
                            Message = $"Something went wrong !",
                        };
                        return result;
                    }
                }
                if (FalseDataCount > 0)
                {
                    result = new MessageService
                    {
                        MessageType = "info",
                        Message = $"{FalseDataCount} of {realDataCount + FalseDataCount} row cannot Imported, Beacause Of Incorrect Data",
                    };
                }
                else
                {
                    result = new MessageService
                    {
                        MessageType = "success",
                        Message = $"{realDataCount} row was Successfully Imported",
                    };
                }
                return result;
            }
        }
        // CSV Processing Action and Return Temp Data Message for Condition
        public static async Task<MessageService> CsvProcess(Stream stream, int TemporaryId)
        {
            MessageService result = new MessageService();
            TblCustomer? customers;
            TblRent? renters;
            int FalseDataCount = 0;
            int realDataCount = 0;
            using (var reader = new StreamReader(stream))
            {
                int LineNumber = 0;
                while (!reader.EndOfStream)
                {
                    LineNumber++;
                    var line = await reader.ReadLineAsync();

                    if (LineNumber == 1)
                    {
                        continue;
                    }
                    var values = line?.Split(',');

                    TemporaryId = Convert.ToInt32(DateTime.Now.Ticks % 100000000) + LineNumber;
                    realDataCount++;
                    try
                    {
                        string? movieTitle = values?[2] ?? string.Empty;

                        customers = new TblCustomer
                        {
                            CusId = TemporaryId,
                            FullName = values?[0],
                            Salutation = values?[3],
                            Address = values?[1],
                            CreatedAt = DateTime.Now
                        };
                        using (var context = new DBContext())
                        {
                            int movieId = await context.TblMovies
                                            .Where(c => c.Title == movieTitle)
                                            .Select(d => d.MvId)
                                            .FirstOrDefaultAsync();
                            if (movieId > 0)
                            {
                                renters = new TblRent
                                {
                                    RentId = TemporaryId,
                                    CusId = TemporaryId,
                                    MvId = movieId,
                                    RentAt = DateTime.Now
                                };

                                context.Add(customers);
                                context.Add(renters);
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                FalseDataCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new MessageService
                        {
                            MessageType = "fail",
                            Message = $"Something went wrong !",
                        };
                        return result;
                    }
                }
                if (FalseDataCount > 0)
                {
                    result = new MessageService
                    {
                        MessageType = "info",
                        Message = $"{FalseDataCount} of {realDataCount + FalseDataCount} row cannot Imported, Beacause Of Incorrect Data",
                    };
                }
                else
                {
                    result = new MessageService
                    {
                        MessageType = "success",
                        Message = $"{realDataCount} row was Successfully Imported",
                    };
                }
                return result;
            }
        }


        // GET: RentController/XlsxExport
        public async Task<ActionResult> XlsxExport(string name = "RentedCustomer")
        {
            List<VwCusMvRent> vwCusMvRent = new List<VwCusMvRent>();
            using (var context = new DBContext())
            {
                vwCusMvRent = await context.VwCusMvRents.ToListAsync();
            }

            var stream = new MemoryStream();
            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add(name);
                var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");
                namedStyle.Style.Font.UnderLine = true;
                namedStyle.Style.Font.Color.SetColor(Color.Blue);
                const int startRow = 1;
                var row = startRow;

                worksheet.Cells["A1"].Value = "Full Name";
                worksheet.Cells["B1"].Value = "Address";
                worksheet.Cells["C1"].Value = "Rented Movie";
                worksheet.Cells["D1"].Value = "Salutation";
                worksheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A1:D1"].Style.Font.Bold = true;

                row = 2;
                foreach (var rents in vwCusMvRent)
                {
                    worksheet.Cells[row, 1].Value = rents.FullName;
                    worksheet.Cells[row, 2].Value = rents.Address;
                    worksheet.Cells[row, 3].Value = rents.Title;
                    worksheet.Cells[row, 4].Value = rents.Salutation;
                    row++;
                }

                xlPackage.Workbook.Properties.Title = "Movie Rented Customer";
                xlPackage.Workbook.Properties.Author = "Movie Rent Shop Co.,Ltd";
                xlPackage.Workbook.Properties.Subject = "Movie Rented Customer";
                xlPackage.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", name+".xlsx");
         
        }

        // GET: RentController/CsvExport
        public async Task<ActionResult> CsvExport(string name = "RentedCustomer")
        {
            List<VwCusMvRent> vwCusMvRent = new List<VwCusMvRent>();
            using (var context = new DBContext())
            {
                vwCusMvRent = await context.VwCusMvRents.ToListAsync();
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            var csv = new StringBuilder();

            // Create Headers
            csv.AppendLine("Full Name,Address,Rented Movie,Salutation");

            // Add data rows
            foreach (var rents in vwCusMvRent)
            {
                csv.AppendLine($"{rents.FullName},{rents.Address},{rents.Title},{rents.Salutation}");
            }

            writer.Write(csv.ToString());
            writer.Flush();
            stream.Position = 0;

            var contentType = "text/csv";
            var fileName = name + ".csv";

            return File(stream, contentType, fileName);
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
        public async Task<ActionResult> Delete(int id)
        {
            TblRent? rent;
            using (var context = new DBContext())
            {
                rent = await context.TblRents.FindAsync(id);
                if (rent != null)
                {
                    context.TblRents.Remove(rent);
                }
                await context.SaveChangesAsync();
            }
            TempData["MessageType"] = "delete";
            TempData["Message"] = "Deteted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
