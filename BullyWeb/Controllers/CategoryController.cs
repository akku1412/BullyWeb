using BullyWeb.Data;
using BullyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BullyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");
                TempData["AlertMessage"] = "Validation Error: DisplayOrder cannot match the Name.";
                return View(obj);
            }

            if (int.TryParse(obj.Name, out _))
            {
                ModelState.AddModelError("Name", "The Name cannot be a number.");
                TempData["AlertMessage"] = "The Name cannot be a number.";
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                if (_db.Categories.Any(c => c.Name == obj.Name))
                {
                    ModelState.AddModelError("Name", "A category with this name already exists.");
                    TempData["AlertMessage"] = "A category with this name already exists.";
                    return View(obj);
                }

                _db.Categories.Add(obj);
                _db.SaveChanges();
          
                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFormDb = _db.Categories.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }

            return View(categoryFormDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");
                TempData["AlertMessage"] = "Validation Error: DisplayOrder cannot match the Name.";
                return View(obj);
            }

            if (int.TryParse(obj.Name, out _))
            {
                ModelState.AddModelError("Name", "The Name cannot be a number.");
                TempData["AlertMessage"] = "The Name cannot be a number.";
                return View(obj);
            }

            if (ModelState.IsValid)
            {
                if (_db.Categories.Any(c => c.Name == obj.Name && c.Id != obj.Id))
                {
                    ModelState.AddModelError("Name", "A category with this name already exists.");
                    TempData["AlertMessage"] = "A category with this name already exists.";
                    return View(obj);
                }

                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFormDb = _db.Categories.Find(id);
            if (categoryFormDb == null)
            {
                return NotFound();
            }

            return View(categoryFormDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["AlertMessage"] = "Invalid category ID.";
                return RedirectToAction("Index");
            }

            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                TempData["AlertMessage"] = "Category not found.";
                return RedirectToAction("Index");
            }

            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
