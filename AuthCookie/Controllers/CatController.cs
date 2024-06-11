using Microsoft.AspNetCore.Mvc;
using AuthCookie.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Diagnostics;


namespace AuthCookie.Controllers
{
    [Authorize]
    public class CatController : Controller
    {
        // GET: CatController
        public ActionResult Index()
        {
            List<Cat> Cats = new List<Cat>();
            using (var context = new DBContext())
            {
                Cats = (from Cat in context.Cats
                        select new Cat
                        {
                            Id = Cat.Id,
                            Name = Cat.Name,
                        }).ToList();
            }

            Cats.ForEach(i => Debug.Write("{0}\t", i.Name));
            return View(Cats);
        }

        // GET: CatController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")] Cat cat)
        {
                using (var context = new DBContext())
                {
                    context.Add(cat);
                    await context.SaveChangesAsync();
                    TempData["successMessage"] = "Created Dog Id and Name Successfully";
                    return RedirectToAction(nameof(Index));
                }
        }


        // GET: CatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CatController/Edit/5
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

        // GET: CatController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CatController/Delete/5
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
