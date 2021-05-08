using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ICachingService _caching;

        public AdminController(ICachingService caching)
        {
            _caching = caching;
        }

        public IActionResult Index()
        {
            return View(_caching.GetCategories());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CategorySaving c)
        {
            _caching.UpsertCategorySaving(c);
            return View();

        }

        public IActionResult Edit(int id)
        {
            CategorySaving category = _caching.GetCategory(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(CategorySaving c)
        {
            _caching.UpsertCategorySaving(c);
            return View();
        }


    }
}
