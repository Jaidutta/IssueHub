using System.ComponentModel;

namespace IssueHub.Models
{
    public class Company
    {
        // Primary Id
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }


        [DisplayName("Company Description")]
        public string Description { get; set; }

        
        // Navigation Properties

        public virtual ICollection<IssueHubUser> Members { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


        // Create a relationship to  invite

        public virtual ICollection<Invite>Invites { get; set; }

    }
}
