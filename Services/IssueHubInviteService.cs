using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueHub.Services
{
    public class IssueHubInviteService : IIssueHubInviteService
    {
        private readonly ApplicationDbContext _context;

        public IssueHubInviteService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AcceptInviteAsync(Guid? token, string userId, int companyId)
        {
            Invite invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

            if (invite == null)
            {
                return false;
            }



            // userId means the user has gone through the registration process

            try
            {
                // because we're accepting the invite, it is no longer valid for anyone else to use
                invite.IsValid = false;

                /*When we first created the invite it only had firstname, lastname and email

                Now we're adding a bit more information about the invite. They now have Fname, LName
                companyId, etc
                */
                invite.InviteeId = userId;

                await _context.SaveChangesAsync();

                return true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddNewInviteAsync(Invite invite)
        {
            try
            {
                await _context.Invites.AddAsync(invite);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> AnyInviteAsync(Guid? token, string email, int companyId)
        { /* We may be passing in the Guid when we send an invite to accept an invite
           * We will take the Guid and will check if there is an invite for the person
           */

            bool result = await _context.Invites.Where(i => i.CompanyId == companyId)
                                            .AnyAsync(i => i.CompanyToken == token &&
                                                           i.InviteeEmail == email);
            return result;
        }

        public async Task<Invite> GetInviteAsync(int inviteId, int companyId)
        {
            try
            {
                Invite invite = await _context.Invites.Where(i => i.CompanyId == companyId)
                                                      .Include(i => i.Company)
                                                      .Include(i => i.Project)
                                                      .Include(i => i.Invitor) // who has invited this person to the company
                                                      .FirstOrDefaultAsync(i => i.Id == inviteId);

                return invite;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Invite> GetInviteAsync(Guid? token, string email, int companyId)
        {
            try
            {
                Invite invite = await _context.Invites.Where(i => i.CompanyId == companyId)
                                                      .Include(i => i.Company)
                                                      .Include(i => i.Project)
                                                      .Include(i => i.Invitor)
                                                      .FirstOrDefaultAsync(i => i.CompanyToken == token && i.InviteeEmail == email);

                return invite;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> ValidateInviteCodeAsync(Guid? token)
        {
            if (token == null)
            {
                return false;
            }

            bool result = false;

            Invite invite = await _context.Invites.FirstOrDefaultAsync(i => i.CompanyToken == token);

            if (invite != null)
            {
                // Determine invite date
                DateTime inviteDate = invite.InviteDate.UtcDateTime;  // invite.InviteDate.UtcDateTime converts the DateTimeOffset to utcDate time

                // Custom validation of invite based on the date it was issued
                // In this case we are allowing an invite to be valid for 7 days
                bool validDate = (DateTime.UtcNow - inviteDate).TotalDays <= 7;

                if (validDate)
                {
                    result = invite.IsValid;
                }
            }

            return result;
        }
    }
}
