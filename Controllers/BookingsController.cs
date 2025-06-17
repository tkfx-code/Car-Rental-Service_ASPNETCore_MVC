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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;


namespace MVC_Project.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookingsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Bookings
        //Fetch List of all Bookings
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
        // Fetch Details of a specific Booking by BookingID
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Car) // Include Car details in the booking
                .Include(b => b.Customer) // Include Customer details in the booking
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingViewModel = _mapper.Map<BookingViewModel>(booking);
            return View(bookingViewModel);
        }

        // GET: Bookings/Create
        //CREATE SHOULD ALWAYS BE CONNECTED TO SPECIFIC CAR LISTING WHEN CLICKING BOOK
        [Authorize]
        public async Task <IActionResult> Create(int carId)
        {
            var car = await _context.CarListings.FirstOrDefaultAsync(car => car.CarId == carId);
            if (car == null)
            {
                return NotFound();
            }

            var email = User.Identity?.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var bookingViewModel = new BookingViewModel 
            { 
                CarId = carId,
                Car = _mapper.Map<CarListingViewModel>(car),
                Customer = _mapper.Map<CustomerViewModel>(customer)
            };
            return View(bookingViewModel);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CarId,StartDate,EndDate,CustomerId")] BookingViewModel bookingViewModel)
        {
            //Get currenty logged in user's CustomerID
            var userEmail = User.Identity?.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == userEmail);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            var car = await _context.CarListings.FindAsync(bookingViewModel.CarId);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            if (ModelState.IsValid)
            {
                //map viewmodel to booking domain model
                var booking = _mapper.Map<Booking>(bookingViewModel);
                booking.CustomerId = customer.CustomerId;
                //booking.Car = car;

                _context.Add(booking);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Booking created successfully."; // Confirmation message after booking
                return RedirectToAction("Index", "Home");
            }
            return View(bookingViewModel);
        }

        // GET: Bookings/Edit/5
        // Fetch the Booking to be edited by BookingID
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b=>b.Car)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }
            var bookingViewModel = _mapper.Map<BookingViewModel>(booking);
            return View(bookingViewModel);
        }

        // POST: Bookings/Edit/5
        // When editing, ensure the BookingId matches the one in the model and return to Index if successful
        //ADD CONFIRMATION MESSAGE
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDate,EndDate")] BookingViewModel bookingViewModel)
        {
            if (id != bookingViewModel.BookingId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(bookingViewModel);
            }

            var booking = await _context.Bookings
                .Include(b=>b.Car)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            _mapper.Map(bookingViewModel, booking);
            booking.Car = await _context.CarListings.FindAsync(bookingViewModel.CarId);
            try
            {
                _context.Update(booking);
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
            TempData["SuccessMessage"] = "Booking updated successfully."; // Confirmation message after editing
            return RedirectToAction(nameof(Index));
        }

        // GET: Bookings/Delete/5
        // Fetch the Booking to be deleted by BookingID
        //AUTHORIZATION : Only logged in users should be able to delete bookings OR admins
        //SHOW CONFIRMATION MESSAGE BEFORE DELETING
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Car) // Include Car details in the booking
                .Include(b => b.Customer) // Include Customer details in the booking
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingViewModel = _mapper.Map<BookingViewModel>(booking);
            return View(bookingViewModel);
        }

        // POST: Bookings/Delete/5
        // Deletes the Booking by BookingID
        //AUTHORIZATION: Only logged in users should be able to delete bookings OR admins
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking deleted successfully."; // Confirmation message after deletion
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
