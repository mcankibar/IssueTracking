using Demo5.Models;
using IssueTracker.DataAccess.Data;
using IssueTracker.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IssueTracker.Controllers
{
    public class PermissionsController : Controller
    {
        public readonly DBContextSample _dBContextSample;

  


        public PermissionsController(DBContextSample dBContextSample)
        {
            _dBContextSample = dBContextSample;
        }
        public IActionResult Index()
        {
            var permissionsList= _dBContextSample.Permissions;
            var users = _dBContextSample.Users.Where(p => p.isDeleted==false).ToList();
            var projects = _dBContextSample.Projects.ToList();



            var viewModel = new PermissionsViewModel();
            foreach (var item in projects)
            {
               viewModel.Projectnames.Add(item.Name);
               
            }
            foreach (var item in users)
            {
                viewModel.Usernames.Add(item.UserName);
            }
            if(permissionsList!=null)
            {
                viewModel.Permissions = permissionsList.ToList();
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(string username, string projectname)
        {
            var user = _dBContextSample.Users.Where(i => i.Email == username).FirstOrDefault();
            var project = _dBContextSample.Projects.Where(i => i.Name == projectname).FirstOrDefault();

            if (user != null && project != null)
            {
                
                var existingPermission = _dBContextSample.Permissions.FirstOrDefault(p => p.User == user && p.Project == project);
                if (existingPermission == null)
                {
                    var newPermission = new Permissions() { User = user, Project = project };
                    
                    _dBContextSample.Permissions.Add(newPermission);
                    _dBContextSample.SaveChanges();
                }
               

               
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            var permission = _dBContextSample.Permissions.FirstOrDefault(p => p.Id == id);

            if (permission != null)
            {
                _dBContextSample.Permissions.Remove(permission);
                _dBContextSample.SaveChanges();
            }

            return RedirectToAction("Index");
        }






    }
}
