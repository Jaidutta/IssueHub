using System.ComponentModel;
using System.Net.Sockets;

namespace IssueHub.Models
{
    public class TIcketHistory
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("Updated Item")]
        public string Property { get; set; }

        [DisplayName("Previous")]
        public string OldValue { get; set; }


        [DisplayName("Current")]
        public string NewValue { get; set; }

        [DisplayName("Date Modified")]
        public DateTimeOffset Created { get; set; }  // To store DateTime in UTC format


        [DisplayName("Description of Change")]
        public string Description { get; set; }


        [DisplayName("Team Member")]
        public int UserId { get; set; }

        // Navigation Properties
        public virtual Ticket Ticket { get; set; }

        public virtual IssueHubUser User { get; set; }
    }
}
