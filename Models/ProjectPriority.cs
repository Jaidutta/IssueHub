using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueHub.Models
{
    public class ProjectPriority
    {
        public int Id { get; set; }

   
        [DisplayName("Project Priority")]
        public string Name { get; set; }


        // Navigation property

        public Project Project { get; set; }
    }
}
