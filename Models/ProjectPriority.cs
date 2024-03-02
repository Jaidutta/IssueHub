using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IssueHub.Models
{
    public class ProjectPriority
    {
        public int Id { get; set; }

   
        [DisplayName("Priority Name")]
        public string Name { get; set; }

    }
}
