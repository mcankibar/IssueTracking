using IssueTracker.Areas.Identity.Data;

namespace Demo5.Models
{
    public class Permissions
    {
        public Guid Id { get; set; }
        public SampleUser User { get; set; }

        public Project Project { get; set; }

       


    }
}
