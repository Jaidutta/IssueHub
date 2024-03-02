using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace IssueHub.Models
{
    public class IssueHubUser: IdentityUser
    {
        [Key]
        public string Id { get; set; }


        [Required]
        [Display(Name = "First Name")] 
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Last Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }


        //Avatar Image Properties

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile AvatarFormFile { get; set; }

        [DisplayName("File Extension")]
        public string AvatarContentType { get; set; }


        [DisplayName("Avatar")]
        public string AvatarFileName { get; set; }

        public byte[] AvatarFileData { get; set; }


        public int? CompanyId { get; set; }


        //Navigation Property
        public virtual Company Company { get; set; }

        public virtual ICollection<Project> Projects { get; set; }


    }
}
