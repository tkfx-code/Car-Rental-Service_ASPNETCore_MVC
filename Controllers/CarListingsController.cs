using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        public CarListingsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: CarListings
        // Fetch List of all Car Listings

        public async Task<IActionResult> Index()
        {
            var cars = await _context.CarListings.ToListAsync();
            var carListingViewModels = _mapper.Map<List<CarListingViewModel>>(cars);
            return View(carListingViewModels);
        }

        // GET: CarListings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.CarListings
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            var carListingViewModel = _mapper.Map<CarListingViewModel>(car);
            return View(carListingViewModel);
        }

        // GET: CarListings/Create
        //AUTHORIZATION: Only admins should be able to create new car listings
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarListings/Create
        //AUTHORIZATION : Only admins should be able to create new car listings
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CarId,Make,Model, PicturesRaw, isAvailable")] CarListingViewModel carListingViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(carListingViewModel.PicturesRaw))
                {
                    // Split the raw input into a list of picture URLs
                    carListingViewModel.Pictures = carListingViewModel.PicturesRaw
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => p.Trim())
                        .ToList();
                }

                var car = _mapper.Map<CarListing>(carListingViewModel);

                _context.Add(car);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Listing created successfully."; // Confirmation message after creation
                return RedirectToAction(nameof(Index));
            }
            return View(carListingViewModel);
        }

        // GET: CarListings/Edit/5
        //AUTHORIZATION: Only admins should be able to edit car listings
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.CarListings.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            var carListingViewModel = _mapper.Map<CarListingViewModel>(car);
            // Convert the list of pictures to a raw string for editing, so we dont have to re-add the images on edit
            carListingViewModel.PicturesRaw = string.Join(Environment.NewLine, carListingViewModel.Pictures ?? new List<string>());
            return View(carListingViewModel);
        }

        // POST: CarListings/Edit/5
        //AUTHORIZATION: Only admins should be able to edit car listings
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model, PicturesRaw, isAvailable")] CarListingViewModel carListingViewModel)
        {
            if (id != carListingViewModel.CarId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(carListingViewModel);
            }

            if (!string.IsNullOrEmpty(carListingViewModel.PicturesRaw))
            {
                // Split the raw input into a list of picture URLs
                carListingViewModel.Pictures = carListingViewModel.PicturesRaw
                    .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToList();
            }

            var car = await _context.CarListings.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            
            _mapper.Map(carListingViewModel, car);
            try
                {
                _context.Update(car);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Listing updated successfully."; // Confirmation message after update
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarListingExists(car.CarId))
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

        // GET: CarListings/Delete/5
        //AUTHORIZATION: Only admins should be able to delete car listings
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.CarListings
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }
            var carListingViewModel = _mapper.Map<CarListingViewModel>(car);
            return View(carListingViewModel);
        }

        // POST: CarListings/Delete/5
        //AUTHORIZATION: Only admins should be able to delete car listings
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.CarListings.FindAsync(id);
            if (car != null)
            {
                _context.CarListings.Remove(car);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Listing deleted successfully."; // Confirmation message after deletion
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CarListingExists(int id)
        {
            return _context.CarListings.Any(e => e.CarId == id);
        }
    }
}
