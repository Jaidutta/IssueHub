using System.ComponentModel;

namespace IssueHub.Models
{
    public class TicketType
    {
        public int Id { get; set; }

        [DisplayName("Type Name")]
        public string Name { get; set; }
    }
}

//It has 1-to-1 relationship with Ticket Entity
