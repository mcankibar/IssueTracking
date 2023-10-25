using Demo5.Models;
using IssueTracker.DataAccess.Data;

namespace IssueTracker.ViewModels
{
    public class PermissionsViewModel
    {

        
        public PermissionsViewModel()
        {
            
            Projectnames = new List<string>();
            Usernames = new List<string>();
        }

        public List<string> Projectnames { get; set; }
        public List<string> Usernames { get; set; }

        public List<Permissions> Permissions { get; set; }


    }

}
