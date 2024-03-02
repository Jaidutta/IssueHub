using NuGet.Protocol.Plugins;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueHub.Models
{
    public class Notification
    {   // Notificaions ae a way to let the members to inform them when things are happening, and also 
        // keep a record that communication has been sent


        // Primary Key
        public int Id { get; set; }


        [DisplayName("Ticket")]
        public int TicketId {  get; set; } // foreign key (notification related to a ticket


        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }


        [Required]
        [DisplayName("Message")]
        public string Message { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTimeOffset Created { get; set; }


        [DisplayName("Recipient")]  // Who the notification is for. It is going to be GUID
        public string RecipientId {  get; set; }


        [DisplayName("Sender")]
        public string SenderId { get; set; }  // Who has sent the notification GUID

        [DisplayName("Has been viewed")]
        public bool Viewed {  get; set; }


        // navigation properties

        public virtual Ticket Ticket { get; set; }

        public virtual IssueHubUser Recipient {  get; set; }

        public virtual IssueHubUser Sender { get; set; }
    }
}
