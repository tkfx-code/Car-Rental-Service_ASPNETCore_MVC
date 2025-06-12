using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project.Data;
using MVC_Project.Model;
using MVC_Project.Models;

namespace MVC_Project.Controllers
{
    public class CarListingsController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public CarListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CarListings
        // Fetch List of all Car Listings
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarListings.ToListAsync());
        }

        // GET: CarListings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carListingViewModel = await _context.CarListings
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (carListingViewModel == null)
            {
                return NotFound();
            }

            return View(carListingViewModel);
        }

        // GET: CarListings/Create
        //AUTHORIZATION: Only admins should be able to create new car listings
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarListings/Create
        //AUTHORIZATION : Only admins should be able to create new car listings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Make,Model,isAvailable")] CarListingViewModel carListingViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carListingViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carListingViewModel);
        }

        // GET: CarListings/Edit/5
        //AUTHORIZATION: Only admins should be able to edit car listings
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carListingViewModel = await _context.CarListings.FindAsync(id);
            if (carListingViewModel == null)
            {
                return NotFound();
            }
            return View(carListingViewModel);
        }

        // POST: CarListings/Edit/5
        //AUTHORIZATION: Only admins should be able to edit car listings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model,isAvailable")] CarListingViewModel carListingViewModel)
        {
            if (id != carListingViewModel.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carListingViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarListingExists(carListingViewModel.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carListingViewModel);
        }

        // GET: CarListings/Delete/5
        //AUTHORIZATION: Only admins should be able to delete car listings
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carListingViewModel = await _context.CarListings
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (carListingViewModel == null)
            {
                return NotFound();
            }

            return View(carListingViewModel);
        }

        // POST: CarListings/Delete/5
        //AUTHORIZATION: Only admins should be able to delete car listings
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carListingViewModel = await _context.CarListings.FindAsync(id);
            if (carListingViewModel != null)
            {
                _context.CarListings.Remove(carListingViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarListingExists(int id)
        {
            return _context.CarListings.Any(e => e.CarId == id);
        }
    }
}
