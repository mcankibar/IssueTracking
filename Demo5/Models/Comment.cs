using IssueTracker.Areas.Identity.Data;

namespace Demo5.Models
{
    public class Comment
    {

        public Guid Id { get; set; }

        public string Text { get; set; }


        public SampleUser CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Guid IssueId { get; set; } 

        public Issue Issue { get; set; }

    }
}
