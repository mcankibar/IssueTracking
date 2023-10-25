using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using IssueTracker.Areas.Identity.Data;
using System;

namespace Demo5.Models
{
    public enum IssueStatus
    {
        Opened,
        Updated,
        Closed
    }

    public class Issue
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        
        public IssueStatus Status { get; set; }

        public SampleUser CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public SampleUser? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
