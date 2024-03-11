using IssueHub.Models;

namespace IssueHub.Services.Interfaces
{
    public interface IIssueHubNotificationService
    {
        public Task AddNotificationAsync(Notification notification);


        public Task<List<Notification>> GetReceivedNotificationsAsync(string userId);


        // Allows Sender See which notifications they have sent
        public Task<List<Notification>> GetSentNotificationsAsync(string userId);


        // Allows to send notification by role
        public Task SentEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);


        // Allows to send notification to specific list of users
        public Task SendMembersEmailNotificationsAsync(Notification notification, List<IssueHubUser> members);


                                             //Notification will include about sender, recipient and all the information  about notification
        public Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject);




    }
}
