using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace IssueHub.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        [DisplayName("Ticket")]
        public int TicketId { get; set; }

        [DisplayName("File Date")]
        public DateTimeOffset Created { get; set; } // To store DateTime in UTC format

        [DisplayName("Team Member")]
        public string UserId { get; set; }


        [DisplayName("File Description")]
        public string Description { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile FormFile { get; set; }   // The IFormFile interface is used for uploading Files in ASP.Net Core MVC.



        [DisplayName("File Name")]
        public string FileName { get; set; }


        public byte[] FileData { get; set; } // bytestream of a physical file


        [DisplayName("File Extension")]
        public string FileContentType { get; set; }


        // Navigation Properties
        public virtual Ticket Ticket { get; set; }
        public virtual IssueHubUser User { get; set; }
    }
}
