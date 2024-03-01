using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueHub.Models
{
    public class Ticket
    {
        // Id in a model's class represent the primary key
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Title")]
        public string Title { get; set; }


        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        public DateTimeOffset Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Updated")]
        public DateTimeOffset? Updated { get; set; }

        [DisplayName("Archived")]
        public bool Archived { get; set; }


        // Nav Ids
        [DisplayName("Project")]
        public int ProjectId { get; set; } // Foreign Key

        [DisplayName("Ticket Type")]
        public int TicketTypeId { get; set; } // Foreign Key


        [DisplayName("Ticket Priority")]
        public int TicketPriorityId { get; set; } // Foreign Key


        [DisplayName("Ticket Status")]
        public int TicketStatusId { get; set; } // Foreign Key


        [DisplayName("Ticket Owner")]
        public string OwnerUserId { get; set; } // Foreign Key. Since they are derivde from Identity, they are of type string(primary key)


        [DisplayName("Ticket Developer")]
        public string DeveloperUserId { get; set; } // Foreign Key. Since they are derivde from Identity, they are of type string(primary key)


        //Navigation Properties
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual IssueHubUser OwnerUser { get; set; }
        public virtual IssueHubUser DeveloperUser { get; set; }


        public virtual ICollection<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new HashSet<TicketAttachment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public virtual ICollection<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();

    }
}
