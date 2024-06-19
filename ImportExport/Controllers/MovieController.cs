using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImportExport.Controllers
{
    public class MovieController : Controller
    {
        // GET: MovieController
        public ActionResult Index()
        {
            return View();
        }
    }
}
