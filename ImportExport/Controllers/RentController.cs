using ImportExport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;
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
        public async Task<ActionResult> Index()
        {
            List<VwCusMvRent> vwCusMvRent = new List<VwCusMvRent>();
            using(var context = new DBContext())
            {
                vwCusMvRent =await context.VwCusMvRents.ToListAsync();
            }
            return View(vwCusMvRent);
        }
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
                            var datetimePrefix = DateTime.Now.ToString("yyyyMMddHHmmss");
                            FilePath = Path.Combine(FolderPath, $"{datetimePrefix}_{SingleFile.FileName}");
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
                            Debug.WriteLine(e.Message);
                            return RedirectToAction(nameof(Index));
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
            int ActualDataCount = 0;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                Debug.WriteLine("Row Count-------------------------------"+ rowCount);
                for (var row = 2; row <= rowCount; row++)
                {
                    TemporaryId = TemporaryId + row;
                    ActualDataCount++;
                    try
                    {
                        string? movieTitle = worksheet?.Cells[row, 3].Value?.ToString() ??  string.Empty;
                        customers = new TblCustomer
                        {
                            CusId = TemporaryId,
                            FullName = worksheet?.Cells[row, 1].Value?.ToString() ?? string.Empty,
                            Salutation = worksheet?.Cells[row, 4].Value?.ToString() ?? string.Empty,
                            Address = worksheet?.Cells[row, 2].Value?.ToString() ?? string.Empty,
                            CreatedAt = DateTime.Now
                        };

                        using (var context = new DBContext())
                        {
                            int MovieId = await IsMovieExist(movieTitle, context);
                            Debug.WriteLine("ksdadfksal;sdfskf------------" + MovieId);
                            if (MovieId != 0)
                            {
                                renters = new TblRent
                                {
                                    RentId = TemporaryId,
                                    CusId = TemporaryId,
                                    MvId = MovieId,
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
                        Debug.WriteLine(ex.Message);
                        result = GenerateErrorMessage("fail", "Something went wrong!");
                        return result;
                    }
                }
                result = GenerateResultMessage(ActualDataCount, FalseDataCount);
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
            int ActualDataCount = 0;
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
                    ActualDataCount++;
                    try
                    {
                        string? movieTitle = values?[2] ?? string.Empty;
                        customers = new TblCustomer
                        {
                            CusId = TemporaryId,
                            FullName = values?[0] ?? string.Empty,
                            Salutation = values?[3] ?? string.Empty,
                            Address = values?[1] ?? string.Empty,
                            CreatedAt = DateTime.Now
                        };
                        using (var context = new DBContext())
                        {
                            int MovieId = await IsMovieExist(movieTitle, context);
                            if (MovieId != 0)
                            {
                                renters = new TblRent
                                {
                                    RentId = TemporaryId,
                                    CusId = TemporaryId,
                                    MvId = MovieId,
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
                        Debug.WriteLine(ex.Message);
                        result = GenerateErrorMessage("fail", "Somethins went wrong !");
                        return result;
                    }
                }
                result = GenerateResultMessage(ActualDataCount, FalseDataCount);
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
                const int StartRow = 1;
                var row = StartRow;

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
        // Generate Result Message For Toast
        private static MessageService GenerateResultMessage(int ActualDataCount, int FalseDataCount)
        {
            if (FalseDataCount > 0)
            {
                return new MessageService
                {
                    MessageType = "info",
                    Message = $"{FalseDataCount} of {ActualDataCount} row(s) could not be imported due to incorrect data."
                };
            }
            else
            {
                return new MessageService
                {
                    MessageType = "success",
                    Message = $"{ActualDataCount- FalseDataCount} row(s) were successfully imported."
                };
            }
        }
        //  Generate  Error Message 
        private static MessageService GenerateErrorMessage(string MessageType, string Message)
        {
            return new MessageService
            {
                MessageType = "fail",
                Message = "Something Went Wrong"
            };
        }
        // Movie Exit Or Not
        private static async Task<int> IsMovieExist(String? MovieTitle, DBContext context)
        {
            int MovieId;
            if (MovieTitle == null)
            {
                return 0;
            }
            MovieId = await context.TblMovies
                        .Where(c => c.Title == MovieTitle)
                        .Select(d => d.MvId)
                        .FirstOrDefaultAsync();
            return MovieId > 0 ? MovieId : 0;
        }
     }
}
