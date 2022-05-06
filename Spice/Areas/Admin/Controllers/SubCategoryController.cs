using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utiltiy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.SubCategory.Include(x => x.Category).ToListAsync());
        }
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await db.SubCategory.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToListAsync()

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);
                if (doesSubCategoryExists.Count() > 0)
                {
                    // error
                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + " Category , Please use another name";
                }
                else
                {
                    db.SubCategory.Add(model.SubCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await (from subCategory in db.SubCategory
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        // GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var subCategoy = await db.SubCategory.FindAsync(id);
            if (subCategoy == null)
                return NotFound();
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = subCategoy,
                SubCategoryList = await db.SubCategory.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToListAsync()

            };
            return View(model);
        }

        // POST - EDIT 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = db.SubCategory.Include(s => s.Category).Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);
                if (doesSubCategoryExists.Count() > 0)
                {
                    // error
                    StatusMessage = "Error : Sub Category exists under " + doesSubCategoryExists.First().Category.Name + "Category , Please use another name";
                }
                else
                {
                    var SubCatFromDb = await db.SubCategory.FindAsync(model.SubCategory.Id);
                    SubCatFromDb.Name = model.SubCategory.Name;
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var subCategoy = await db.SubCategory.FindAsync(id);
            if (subCategoy == null)
                return NotFound();
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = subCategoy,
                SubCategoryList = await db.SubCategory.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToListAsync()

            };

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            var subCategoy = await db.SubCategory.FindAsync(id);
            if (subCategoy == null)
                return NotFound();
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = subCategoy,
                SubCategoryList = await db.SubCategory.OrderBy(x => x.Name).Select(x => x.Name).Distinct().ToListAsync()

            };
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await db.SubCategory.FindAsync(id);
            if (subCategory == null)
                return NotFound();
      
            db.SubCategory.Remove(subCategory);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
