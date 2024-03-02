using System.ComponentModel;

namespace IssueHub.Models
{
    public class Invite
    {
        //primary key
        public int Id { get; set; }

        [DisplayName("Date Sent")]
        public DateTimeOffset InviteDate { get; set; }


        [DisplayName("Join Date")]
        public DateTimeOffset JoinDate { get; set; }


        [DisplayName("Code")]
        public Guid CompanyToken { get; set; }


        // Foreign Keys
        [DisplayName("Company")]
        public int CompanyId { get; set; }


        [DisplayName("Project")]
        public int ProjectId { get; set; }


        [DisplayName("Invitor")]
        public string InvitorId {  get; set; }


        [DisplayName("Invitee")]
        public string InviteeId { get; set; }


        [DisplayName("Invitee Email")]
        public string InviteeEmail { get; set; }


        [DisplayName("Invitee First Name")]
        public string InviteeFirstName { get; set; }


        [DisplayName("Invitee Last Name")]
        public string InviteeLastName { get; set; }


        // used to determine if a particular invite is valid or not. 
        public bool IsValid {  get; set; } 



        // navigation properties

        public virtual Company Company { get; set; }

        public virtual Project Project { get; set;  }      
        
        public virtual IssueHubUser Invitor { get; set; }

        public virtual IssueHubUser Invitee { get; set; }


    }
}
