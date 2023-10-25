
using Demo5.Models;
using IssueTracker.DataAccess.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Demo5.Controllers
{
    public class CommentController : Controller
    {

        public readonly DBContextSample _dBContextSample;




        public CommentController(DBContextSample dBContextSample)
        {
            _dBContextSample = dBContextSample;


        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddComment(string commentText, Guid issueId)
        {
            if (ModelState.IsValid)
            {
                
                var loggedInUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(loggedInUserId))
                {
                    var issue = _dBContextSample.Issues.FirstOrDefault(i => i.Id == issueId);

                    issue.UpdatedDate = DateTime.UtcNow;

                   issue.UpdatedBy= _dBContextSample.Users.FirstOrDefault(i=>i.Id==loggedInUserId);

                    issue.Status = (IssueStatus)1;


                    if (issue != null)
                    {
                        
                        var comment = new Comment
                        {
                            Text = commentText,
                            IssueId = issue.Id,
                            CreatedDate = DateTime.Now,
                            CreatedBy = _dBContextSample.Users.FirstOrDefault(u => u.Id == loggedInUserId)
                        };

                       
                        _dBContextSample.Comments.Add(comment);
                        _dBContextSample.SaveChanges();

                        
                        return RedirectToAction("IssueDetail", "Issue", new { id = issue.Id });
                    }
                }
            }

           
            return View("Error");
        }

       
        [HttpPost]
        public IActionResult EditComment(Guid commentId, string commentText)
        {
            var comment = _dBContextSample.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = commentText;
            _dBContextSample.SaveChanges();

            return RedirectToAction("IssueDetail", "Issue", new { id = comment.IssueId });
        }


        [HttpPost]
        public IActionResult DeleteComment(Guid commentId)
        {
            // commentId'yi kullanarak yorumu silin
            var comment = _dBContextSample.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment != null)
            {
                _dBContextSample.Comments.Remove(comment);
                _dBContextSample.SaveChanges();
            }

            return RedirectToAction("IssueDetail", "Issue", new { id = comment.IssueId });
        }







    }
}
