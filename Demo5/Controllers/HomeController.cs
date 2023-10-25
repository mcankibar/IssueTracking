using Demo5.Models;
using IssueTracker.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Demo5.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContextSample _dbContextSample;

        public HomeController(ILogger<HomeController> logger, DBContextSample dbContextSample)
        {
            _logger = logger;
            _dbContextSample = dbContextSample;
        }

        public async Task<IActionResult> Index()
        {
            
            var issues = await _dbContextSample.Issues.ToListAsync();

            return View(issues);
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