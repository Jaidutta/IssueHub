using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IssueHub.Services
{
    public class IssueHubCompanyInfoService : IIssueHubCompanyInfoService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IssueHubUser> _userManager;
        public IssueHubCompanyInfoService(ApplicationDbContext context,
                    UserManager<IssueHubUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<IssueHubUser>> GetAllMemberAsync(int companyId)
        {
            List<IssueHubUser> result = new();

            result = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

            return result;
        }

        public async Task<List<Project>> GetAllProjectsAsync(int companyId)
        {
            List<Project> result = new();
            result = await _context.Projects.Where(p => p.CompanyId == companyId)
                                   .Include(p => p.ProjectPriority)
                                   .Include(p => p.Members)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.Comments)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.History)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.Attachments)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.Notifications)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.DeveloperUser)
                                    .Include(p => p.Tickets)
                                        .ThenInclude(t => t.OwnerUser)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.TicketPriority)
                                   .Include(p => p.Tickets)
                                        .ThenInclude(t => t.TicketStatus)
                                   .Include(p => p.Tickets)
                                         .ThenInclude(t => t.TicketType)
                                   .ToListAsync();

            return result;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync(int companyId)
        {
            List<Ticket> result = new();
            List<Project> projects = new();

            projects = await GetAllProjectsAsync(companyId);

            result = projects.SelectMany(p => p.Tickets).ToList();

            return result;
        }

        public async Task<Company> GetCompanyInfoByIdAsync(int? companyId)
        {
            Company result = new();
        //https://stackoverflow.com/questions/1024559/when-to-use-first-and-when-to-use-firstordefault-with-linq
            if (companyId != null)
            {
                result = await _context.Companies
                                        .Include(c => c.Members)
                                        .Include(c => c.Projects)
                                        .Include(c => c.Invites)
                                        .FirstOrDefaultAsync(c => c.Id == companyId);
            }

            // if company isn't found it will return the instantiated company object
            return result;
        }
    }
}
