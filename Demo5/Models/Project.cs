using IssueTracker.Areas.Identity.Data;

namespace Demo5.Models
{
    public class Project
    {


        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public List<Issue> Issues { get; set; }

       

    }


    
}
