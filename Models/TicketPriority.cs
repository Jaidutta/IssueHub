using System.ComponentModel;

namespace IssueHub.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }

        [DisplayName("Priority Name")]
        public string Name { get; set; }
    }
}

//It has a 1-to-1 relationshiip with the Ticket Entity