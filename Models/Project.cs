using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IssueHub.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DisplayName("Company")]
        public int? CompanyId { get; set; } // Foreign key


        [Required]
        [StringLength(50)]
        [DisplayName("Project Name")]
        public string Name { get; set; }


        [DisplayName("Project Description")]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        public DateTimeOffset StartDate { get; set; }


        [DataType(DataType.Date)]
        public DateTimeOffset EndDate { get; set; }

        public int? ProjectPriorityId { get; set; }  // Foreign key


        [DisplayName("Archived")]
        public bool Archived { get; set; }


        // Image Properties
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile ImageFormFile { get; set; }   // The IFormFile interface is used for uploading Files in ASP.Net Core MVC.



        [DisplayName("File Name")]
        public string ImageFileName { get; set; }


        public byte[] ImageFileData { get; set; } // bytestream of a physical file


        [DisplayName("File Extension")]
        public string ImageContentType { get; set; }

        //----------Image Properties Ends-----------------

        // Navigation Properties

        public virtual  Company Company { get; set; } 

        public virtual ProjectPriority ProjectPriority { get; set; }


        public ICollection<IssueHubUser> Members { get; set; } = new HashSet<IssueHubUser>();

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
