using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace IssueHub.Services
{
    public class IssueHubNotificationService : IIssueHubNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IssueHubRolesService _rolesService;

        public IssueHubNotificationService(ApplicationDbContext context, 
            IEmailSender emailSender, 
            IssueHubRolesService rolesService)
        {
            _context = context;
            _emailSender = emailSender;
            _rolesService = rolesService;
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        // If I am a developer and I have been assigned a ticket, I am sent a notification. The developer
        // has become the recipient of the notification. I can query the db for all of the notifications
        public async Task<List<Notification>> GetReceivedNotificationsAsync(string userId)
        {
            try
            {                                     // With all queries we don't have to include all the info
                                                  // We need to think about what we want show with a notification
                List<Notification> notifications = await _context.Notifications
                                                            .Include(n => n.Ticket)
                                                            .Include(n => n.Recipient)
                                                            .Include(n => n.Sender)
                                                                .ThenInclude(t => t.Projects)
                                                            .Where(n => n.RecipientId == userId)
                                                            .ToListAsync();
                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Notification>> GetSentNotificationsAsync(string userId)
        {
            try
            {                                     // With all queries we don't have to include all the info
                                                  // We need to think about what we want show with a notification
                List<Notification> notifications = await _context.Notifications
                                                            .Include(n => n.Ticket)
                                                            .Include(n => n.Recipient)
                                                            .Include(n => n.Sender)
                                                                .ThenInclude(t => t.Projects)
                                                            .Where(n => n.SenderId == userId)
                                                            .ToListAsync();
                return notifications;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject)
        {
            IssueHubUser issueHubUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == notification.RecipientId);

            if (issueHubUser != null)
            {
                string issueHubUserEmail = issueHubUser.Email;
                string message = notification.Message;

                await _emailSender.SendEmailAsync(issueHubUserEmail, emailSubject, message);

                return true;
            }

            else
            {
                return false;
            }
        }

        
        public async Task SentEmailNotificationsByRoleAsync(Notification notification, int companyId, string role)
        {
            try
            {
                List<IssueHubUser> members = await _rolesService.GetUsersNotInRoleAsync(role, companyId);

                foreach(IssueHubUser issueHubUser in members)
                {
                    notification.RecipientId = issueHubUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendMembersEmailNotificationsAsync(Notification notification, List<IssueHubUser> members)
        {
            try
            {
                foreach (IssueHubUser issueHubUser in members)
                {
                    notification.RecipientId = issueHubUser.Id;
                    await SendEmailNotificationAsync(notification, notification.Title);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
