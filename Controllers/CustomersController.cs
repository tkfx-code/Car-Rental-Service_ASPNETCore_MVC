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
        //AUTHORIZATION : Only admins can see list of customers - show all info
        public async Task<IActionResult> Index()
        {
            //foreach (Customer c in _context.Customers)
            //{
            //    var customerVM = new CustomerViewModel();
            //    customerVM.CustomerId = c.CustomerId;
            //    customerVM.FirstName = c.FirstName;
            //    customerVM.LastName = c.LastName;
            //    customerVM.Email = c.Email;
            //    customerVM.PhoneNumber = c.PhoneNumber;
            //    customerViewModels.Add(customerVM);
            //}
            var customers = await _context.Customers.ToListAsync();
            customerViewModels = _mapper.Map<List<CustomerViewModel>>(customers);
            return View(customerViewModels);
        }

        // GET: Customers/Details/5
        //AUTHORIZATION : Only logged in users should be able to view THEIR customer details + Admins
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.Include(c => c.Customers)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            //customerViewModel.CustomerId = customer.CustomerId;
            //customerViewModel.FirstName = customer.FirstName;
            //customerViewModel.LastName = customer.LastName;
            //customerViewModel.Email = customer.Email;
            //customerViewModel.PhoneNumber = customer.PhoneNumber;

            if (customerViewModel == null)
            {
                return NotFound();
            }

            return View(customerViewModel);
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
                var customer = new Customer();
                customer.CustomerId = customerViewModel.CustomerId;
                customer.FirstName = customerViewModel.FirstName;
                customer.LastName = customerViewModel.LastName;
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        //AUTHORIZATION: Only admins and the own customer should be able to edit customer details
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            //var customerViewModel = new CustomerViewModel();
            //customerViewModel.CustomerId = customer.CustomerId;
            //customerViewModel.FirstName = customer.FirstName;
            //customerViewModel.LastName = customer.LastName;
            //customerViewModel.Email = customer.Email;
            //customerViewModel.PhoneNumber = customer.PhoneNumber;

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
                    //var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
                    //customer.FirstName = customerViewModel.FirstName;
                    //customer.LastName = customerViewModel.LastName;
                    //customer.Email = customerViewModel.Email;
                    //customer.PhoneNumber = customerViewModel.PhoneNumber;
                    //_context.Update(customer);

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
        //AUTHORIZATION: Only admins should be able to delete customers
        // SHOW CONFIRMATION MESSAGE BEFORE DELETING
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
            //var customerViewModel = new CustomerViewModel();
            //customerViewModel.Id = custoner.Id;
            //customerViewModel.LastName = customer.LastName;
            //customerViewModel.FirstName = customer.FirstName;
            //customerViewModel.Email = customer.Email;
            //customerViewModel.PhoneNumber = customer.PhoneNumber;

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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerViewModelExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
