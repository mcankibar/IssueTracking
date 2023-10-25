using IssueTracker.Areas.Identity.Data;
using Demo5.Models;
using IssueTracker.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo5.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {

        public readonly DBContextSample _dBContextSample;


        public ProjectController(DBContextSample dBContextSample)
        {
            _dBContextSample = dBContextSample;
        }
        public IActionResult Index()
        {
            var user = _dBContextSample.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

            IEnumerable<Project> objProjectsList;
            if (User.IsInRole("Admin"))
            {
                
                objProjectsList = _dBContextSample.Projects;
            }
            else if (User.IsInRole("Customer"))
            {
                
                var userProjects = _dBContextSample.Permissions
                    .Where(p => p.User == user)
                    .Select(p => p.Project)
                    .ToList();

                objProjectsList = userProjects;
            }
            else
            {
               
                objProjectsList = new List<Project>();
            }

            return View(objProjectsList);
        }



        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public IActionResult Create(Project obj)
        {


            
            Guid projectId = Guid.NewGuid();

         
            obj.Id = projectId;

           
            obj.Issues=new List<Issue>();

            _dBContextSample.Projects.Add(obj);
            _dBContextSample.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            
            var projectToDelete = _dBContextSample.Projects.FirstOrDefault(p => p.Id == id);

            if (projectToDelete != null)
            {
                
                _dBContextSample.Projects.Remove(projectToDelete);
                _dBContextSample.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult ProjectIssueList(Guid id)
        {
            var projectId = id.ToString().ToLower();

            var project = _dBContextSample.Projects
                .Include(p => p.Issues) 
                .FirstOrDefault(p => p.Id.ToString().ToLower() == projectId);

            if (project != null)
            {
                var projectIssueList = project.Issues;
                return View(projectIssueList);
            }

            
            return RedirectToAction("Error");


        }

    }
}
