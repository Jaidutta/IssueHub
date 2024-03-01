using System.ComponentModel;

namespace IssueHub.Models
{
    public class TicketComment
    {
        // Id
        public int Id { get; set; }

        // Comment
        [DisplayName("Member Comment")]
        public string Comment { get; set; }

        // TicketId

        [DisplayName("Ticket")]
        public int TicketId { get; set; }


        // UserId
        [DisplayName("Team Member")]
        public string UserId { get; set; }

        // Created
        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }  // To store DateTime in UTC format

        //--------------- Navigation Properties----------------

        // Ticket
        public virtual Ticket Ticket { get; set; }

        // User
        public virtual IssueHubUser User { get; set; }
    }
}
