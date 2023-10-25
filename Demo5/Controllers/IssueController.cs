

using Demo5.Models;
using IssueTracker.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IssueTracker.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {

        public readonly DBContextSample _dBContextSample;
     



        public IssueController(DBContextSample dBContextSample)
        {
            _dBContextSample = dBContextSample;
           
            
        }


        public IActionResult Index()
        {
            
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            
            if (User.IsInRole("Admin"))
            {
                
                var objIssuesList = _dBContextSample.Issues
                    .Include(issue => issue.CreatedBy)
                    .Include(issue => issue.Project)
                    .ToList();

                return View(objIssuesList);
            }
            else if (User.IsInRole("Customer"))
            {
                if (_dBContextSample.Permissions !=null)
                {
                    var permittedProjectIds = _dBContextSample.Permissions
                  .Where(permission => permission.User.Id == loggedInUserId)
                  .Select(permission => permission.Project.Id)
                  .ToList();


                    var objIssuesList = _dBContextSample.Issues
                        .Include(issue => issue.CreatedBy)
                        .Include(issue => issue.Project)
                        .Where(issue => permittedProjectIds.Contains(issue.Project.Id))
                        .ToList();

                    return View(objIssuesList);
                }
                else
                {

                    return View(null);
                }
                   
              
               

                
            }

            
            return RedirectToAction("Error");
        }


        //GET
        public IActionResult Create()
        {
            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

           
            if (User.IsInRole("Customer"))
            {
                
                var permittedProjectIds = _dBContextSample.Permissions
                    .Where(permission => permission.User.Id == loggedInUserId)
                    .Select(permission => permission.Project.Id)
                    .ToList();

                var permittedProjects = _dBContextSample.Projects
                    .Where(project => permittedProjectIds.Contains(project.Id))
                    .ToList();

                var projectListItems = permittedProjects.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();

                ViewData["Projects"] = projectListItems;

                return View();
            }
            else
            {
                
                var projects = _dBContextSample.Projects.ToList();
                var projectListItems = projects.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
                ViewData["Projects"] = projectListItems;

                return View();
            }
        }



        //POST
        [HttpPost]
        public IActionResult Create(Issue obj)
        {
            var selectedProjectId = obj.ProjectId;
            var selectedProject = _dBContextSample.Projects
                .Include(p => p.Issues)
                .FirstOrDefault(p => p.Id == selectedProjectId);

            selectedProject.Issues.Add(obj);

            var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(loggedInUserId))
            {
                var loggedInUser = _dBContextSample.Users.FirstOrDefault(u => u.Id == loggedInUserId);

                if (loggedInUser != null)
                {
                    obj.CreatedBy = loggedInUser;
                }
            }

            obj.CreatedDate = DateTime.Now;
            obj.Status = IssueStatus.Opened; 

            _dBContextSample.Issues.Add(obj);
            _dBContextSample.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult IssueDetail(Guid id)
        {
            var issue = _dBContextSample.Issues
    .Include(i => i.CreatedBy)
    .Include(i => i.Comments)
        .ThenInclude(c => c.CreatedBy) // Yorumların yaratıcılarını Include etmek için
    .FirstOrDefault(i => i.Id == id);




            if (issue == null)
            {
                
                return RedirectToAction("Error");
            }
         

            return View(issue);
        }


        [HttpPost]
        public IActionResult EditIssueText(Guid issueId, string issueText)
        {
            var issue = _dBContextSample.Issues.FirstOrDefault(c => c.Id == issueId);

            if (issue == null)
            {
                return NotFound();
            }

            issue.Description = issueText;
            _dBContextSample.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult DeleteIssue(Guid issueId)
        {
           
            var issue = _dBContextSample.Issues.FirstOrDefault(c => c.Id == issueId);

            if (issue != null)
            {
                _dBContextSample.Issues.Remove(issue);
                _dBContextSample.SaveChanges();
            }

            return RedirectToAction("Index");
        }








    }
}
