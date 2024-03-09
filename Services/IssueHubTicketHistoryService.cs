using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IssueHub.Services
{
    public class IssueHubTicketHistoryService : IIssueHubTicketHistoryService
    {
        private readonly ApplicationDbContext _context;

        public IssueHubTicketHistoryService(ApplicationDbContext context) {
            _context = context;
        }
        public async Task AddHistoryAsync(Ticket oldTicket, Ticket newTicket, string userId)
        {
            /* Whatever is in the database as our old ticket, we are going to take whats been modified 
             * about that ticket as our new ticket. We are going to compare. We are going to compare
             * what has been changed about that ticket and will save to the db a history based on what
             * has been changed. old ticket and new ticket reference the exact same ticket. Ticket that 
             * has the same id. Old ticket id is equal to the new ticket id
             * 
             */

            // NEW TICKET HAS BEEN ADDED
            if(oldTicket == null && newTicket != null)
            {
                TicketHistory history = new()
                {
                    TicketId = newTicket.Id,
                    Property = "",
                    OldValue = "", 
                    NewValue = "",
                    Created = DateTime.Now,
                    UserId = userId,
                    Description = "New Ticket Created"

                };

                try
                {
                    await _context.TicketHistories.AddAsync(history);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else
            {
                // Check ticket title
                if(oldTicket.Title != newTicket.Title)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Title",
                        OldValue = oldTicket.Title,
                        NewValue = newTicket.Title,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket title: {newTicket.Title}"

                    };

                    await _context.TicketHistories.AddAsync(history); 
                   
                }

                // Check ticket Description
                if (oldTicket.Description != newTicket.Description)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Description",
                        OldValue = oldTicket.Description,
                        NewValue = newTicket.Description,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket Description: {newTicket.Description}"

                    };

                    await _context.TicketHistories.AddAsync(history);

                }

                // Check if ticket priority changed by checking tickeTPriorityID
                // --> if they are not equal then they were modified
                if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketPriority",
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = newTicket.TicketPriority.Name,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket Priority: {newTicket.TicketPriority.Name}"

                    };

                    await _context.TicketHistories.AddAsync(history);

                }


                // Check if ticket status changed by checking TicketStatusId
                // --> if they are not equal then the TicketStatus was changed
                if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketStatus",
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = newTicket.TicketStatus.Name,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket Status: {newTicket.TicketStatus.Name}"

                    };

                    await _context.TicketHistories.AddAsync(history);

                }

                // Check if TicketType changed by checking TicketTypeId
                // --> if they are not equal then the TicketType was changed
                if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "TicketStatus",
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = newTicket.TicketType.Name,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket TicketType: {newTicket.TicketType.Name}"

                    };

                    await _context.TicketHistories.AddAsync(history);

                }

                // Check if Developer changed by checking DeveloperId
                // --> if they are not equal then the Developer was changed
                // It can be that there was no developer before or the developer has changed

                if (oldTicket.DeveloperUserId != newTicket.DeveloperUserId)
                {
                    TicketHistory history = new()
                    {
                        TicketId = newTicket.Id,
                        Property = "Developer",

                        // If the DeveloperUser.FullName is null then set the FullName = "Not Assigned"
                        // meaning this is the first time ticket has been assigned
                        OldValue = oldTicket.DeveloperUser?.FullName?? "Not Assigned", 
                        NewValue = newTicket.DeveloperUser?.FullName,
                        Created = DateTime.Now,
                        UserId = userId,
                        Description = $"New Ticket Developer: {newTicket.DeveloperUser.FullName}"

                    };

                    await _context.TicketHistories.AddAsync(history);

                }

                try
                {
                    // Save the TicketHistory DataBaseSet to the database
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            
        }

        public async Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int companyId)
        {
            try
            {
                List<Project> projects = (await _context.Companies.Include(c => c.Projects)
                                                                    .ThenInclude(p => p.Tickets) // ThenInclude allows to get to the foreign id property of the Project
                                                                        .ThenInclude(t => t.History)  // ThenInclude allows to get to the foreign id property of the Ticket
                                                                           .ThenInclude(h => h.User)  // ThenInclude allows to get to the foreign id property of the History
                                                                 .FirstOrDefaultAsync(c => c.Id == companyId))
                                                                 .Projects.ToList();

                List<Ticket> tickets = projects.SelectMany(p => p.Tickets).ToList();
                List<TicketHistory> ticketHistories = tickets.SelectMany(t => t.History).ToList();

                return ticketHistories;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId)
        {
            try
            {
                Project project = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                    .Include(p => p.Tickets)
                                                        .ThenInclude(t => t.History)
                                                            .ThenInclude(h => h.User)
                                                    .FirstOrDefaultAsync(p => p.Id == projectId);

               List<TicketHistory> ticketHistory = project.Tickets.SelectMany(t => t.History).ToList();
               
                return ticketHistory;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
