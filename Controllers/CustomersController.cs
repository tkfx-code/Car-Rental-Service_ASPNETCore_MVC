using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Project.Data;
using MVC_Project.Model;
using MVC_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace MVC_Project.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private List<CustomerViewModel> customerViewModels = new List<CustomerViewModel>();

        public CustomersController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Customers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            
            var customers = await _context.Customers.ToListAsync();
            customerViewModels = _mapper.Map<List<CustomerViewModel>>(customers);
            return View(customerViewModels);
        }

        // GET: Customers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.Include(c => c.Bookings)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
           

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(customerViewModel);
        }

        [Authorize]
        // GET: Customers/Profile
        //See current users profile
        public async Task<IActionResult> Profile()
        {
            //Use .Name since ASPNETCORE uses email as log in credentials
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("Admin"))
            {
                // If the user is an admin, redirect to the admin index
                return RedirectToAction("Admin", "Home");
            }

            var customer = await _context.Customers
                .Include(c=>c.Bookings)
                .ThenInclude(b => b.Car) 
                .FirstOrDefaultAsync(c => c.Email == userEmail);

            if (customer == null)
            {
                // For debugging: show message
                TempData["ErrorMessage"] = "There is no customer profile to show.";
                return RedirectToAction("Index", "Home");
            }

            var customerVM = _mapper.Map<CustomerViewModel>(customer);
            var bookingsVM = _mapper.Map<List<BookingViewModel>>(customer.Bookings);
            
            var profileVM = new ProfileViewModel
            {
                Customer = customerVM,
                Bookings = bookingsVM
            };

            return View(profileVM);
        }


        // GET: Customers/Create
        //ADMINS CAN CREATE USERS
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FirstName,LastName,Email,PhoneNumber")] CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = _mapper.Map<Customer>(customerViewModel);
                _context.Add(customer);
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        //AUTHORIZATION: Only admins and the own customer should be able to edit customer details
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            

            if (customer == null)
            {
                return NotFound();
            }
            return View(customerViewModel);
        }

        // POST: Customers/Edit/5
        //AUTHOIZATION
        //SHOW CONFIRMATION MESSAGE AFTER EDITING
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FirstName,LastName,Email,PhoneNumber")] CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var customer = await _context.Customers.FindAsync(id);
                    _mapper.Map(customerViewModel, customer);
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerViewModelExists(customerViewModel.CustomerId))
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
            return View(customerViewModel);
        }

        // GET: Customers/Delete/5
        // SHOW CONFIRMATION MESSAGE BEFORE DELETING
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(customerViewModel);
        }

        // POST: Customers/Delete/5
        //AUTHORIZATION: Only admins should be able to delete customers
        // SHOW CONFIRMATION MESSAGE AFTER DELETING
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Profile deleted successfully."; // Confirmation message after deletion
            }
            return RedirectToAction("Admin", "Home");
        }

        private bool CustomerViewModelExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
