using Microsoft.AspNetCore.Mvc;
using AuthCookie.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Cat? cat;
            using(DBContext context = new DBContext())
            {
                cat = await context.Cats.FirstOrDefaultAsync(c => c.Id == id);
            }

            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
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
            if (string.IsNullOrWhiteSpace(cat.Name))
            {
                ModelState.AddModelError("Name", "Please Fill Name");
                return View();
            }

            if (ModelState.IsValid)
            {
                using (var context = new DBContext())
                {
                    context.Add(cat);
                    await context.SaveChangesAsync();
                    TempData["MessageType"] = "success";
                    TempData["Message"] = "Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
                        
            return View();
        }

        // GET: CatController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cat? cat;
            using (var context = new DBContext())
            {
               cat = (from Cat in context.Cats
                        where Cat.Id == id
                        select new Cat
                        {
                            Id = Cat.Id,
                            Name = Cat.Name,
                        }).FirstOrDefault();
            }
            if(cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }

        // POST: CatController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name")] Cat cat)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
            {
                ModelState.AddModelError("Name", "Please Fill Name");
                return View();
            }
            if (id != cat.Id)
            {
                ModelState.AddModelError("Name", "Your Name is Not in Our System");
                return View();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBContext context = new DBContext())
                    {
                        context.Update(cat);
                        await context.SaveChangesAsync();
                        TempData["MessageType"] = "success";
                        TempData["Message"] = "Updated Successfully";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExists(cat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["MessageType"] = "fail";
                        TempData["Message"] = "Updated Fail";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(cat);
        }

        // GET: CatController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Cat? cat;
            using (DBContext context = new DBContext()) 
            {
                cat = await context.Cats.FindAsync(id);
                if (cat != null)
                {
                    context.Cats.Remove(cat);
                }
                await context.SaveChangesAsync();
            }
            TempData["MessageType"] = "delete";
            TempData["Message"] = "Deteted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CatExists(int id)
        {
            using(DBContext context = new DBContext())
            {
                return context.Cats.Any(e => e.Id == id);
            }   
        }
    }
}
