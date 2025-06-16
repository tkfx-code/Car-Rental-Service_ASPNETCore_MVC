using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_Project.Models;
using MVC_Project.Model;
using MVC_Project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace MVC_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var carListings = _context.CarListings.ToList();
            return View(carListings);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            var customers = _context.Customers
                .ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            var bookings = _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Car)
                .ProjectTo<BookingViewModel>(_mapper.ConfigurationProvider)
                .ToList();

            var adminViewModel = new AdminViewModel
            {
                Customers = customers,
                Bookings = bookings
            };
            return View(adminViewModel);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
