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
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        //Fetch List of all Bookings
        //AUTHORIZATION: Only logged in ADMINS should be able to view all bookings
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        // Fetch Details of a specific Booking by BookingID
        //AUTHORIZATION: Only logged in users should be able to view booking details 
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingViewModel = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookingViewModel == null)
            {
                return NotFound();
            }

            return View(bookingViewModel);
        }

        // GET: Bookings/Create
        // Display the Create Booking form - with dropdown menu of all the cars(?)
        //CREATE SHOULD ALWAYS BE CONNECTED TO SPECIFIC CAR LISTING WHEN CLICKING BOOK
        //AUTHORIZATION: Only logged in users should be able to create bookings 
        public IActionResult Create()
        {
            ViewBag.Customers = new SelectList(_context.CarListings, "CarId", "Make", "Model"); 
            return View();
        }

        // POST: Bookings/Create
        //Bind CarId and BookingId to the bookingViewModel, CustomerID should already be bound due to log in
        //CONFIRMATION MESSAGE WHEN BOOKING IS SUCCESSFUL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,BookingId,StartDate,EndDate,TotalPrice")] BookingViewModel bookingViewModel)
        {
            Customer customer = _context.Customers.Find(bookingViewModel.BookingId); //unsure if fetching/binding the correct data?
            bookingViewModel.Customer = customer;
            if (ModelState.IsValid)
            {
                _context.Add(bookingViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookingViewModel);
        }

        // GET: Bookings/Edit/5
        // Fetch the Booking to be edited by BookingID
        //AUTHORIZATION: Only logged in users should be able to edit bookings OR admins
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingViewModel = await _context.Bookings.FindAsync(id);
            if (bookingViewModel == null)
            {
                return NotFound();
            }
            return View(bookingViewModel);
        }

        // POST: Bookings/Edit/5
        // When editing, ensure the BookingId matches the one in the model and return to Index if successful
        //ADD CONFIRMATION MESSAGE
        //AUTHORIZATION: Only logged in users should be able to edit bookings OR admins
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDate,EndDate,TotalPrice")] BookingViewModel bookingViewModel)
        {
            if (id != bookingViewModel.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(bookingViewModel.BookingId))
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
            return View(bookingViewModel);
        }

        // GET: Bookings/Delete/5
        // Fetch the Booking to be deleted by BookingID
        //AUTHORIZATION : Only logged in users should be able to delete bookings OR admins
        //SHOW CONFIRMATION MESSAGE BEFORE DELETING
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingViewModel = await _context.Bookings
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookingViewModel == null)
            {
                return NotFound();
            }

            return View(bookingViewModel);
        }

        // POST: Bookings/Delete/5
        // Deletes the Booking by BookingID
        //AUTHORIZATION: Only logged in users should be able to delete bookings OR admins
        //SHOW CONFIRMATION MESSAGE AFTER DELETING
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingViewModel = await _context.Bookings.FindAsync(id);
            if (bookingViewModel != null)
            {
                _context.Bookings.Remove(bookingViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
