using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Models.Enums;
using IssueHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IssueHub.Services
{
    public class IssueHubTicketService : IIssueHubTicketService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIssueHubRolesService _rolesService;
        private readonly IIssueHubProjectService _projectService;

        public IssueHubTicketService(ApplicationDbContext context, 
                    IIssueHubRolesService rolesService, 
                    IIssueHubProjectService projectService) 
        {
            _context = context;
            _rolesService = rolesService;
            _projectService = projectService;
        }
        // CRUD --> Create Ticket
        public async Task AddNewTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        // CRUD --> Archive/Delete Ticket
        public async Task ArchiveTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.Archived = true;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AssignTicketAsync(int ticketId, string userId)
        {
            Ticket ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            try
            {
                if(ticket != null)
                {   
                    // if we find the ticket, then set the ticket's developerUserId to the userId we are passing in
                    ticket.DeveloperUserId = userId;


                    // We will revisit this code when we assign ticket

                    ticket.TicketStatusId = (await LookupTicketStatusIdAsync("Development")).Value;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByCompanyAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = await _context.Projects
                    .Where(p => p.CompanyId == companyId)
                    .SelectMany(p => p.Tickets)
                    .Include(t => t.Attachments)
                    .Include(t => t.Comments)
                    .Include(t => t.DeveloperUser)
                    .Include(t => t.History)
                    .Include(t => t.OwnerUser)
                    .Include(t => t.TicketPriority)
                    .Include(t => t.TicketStatus)
                    .Include(t => t.Project)
                    .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByPriorityAsync(int companyId, string priorityName)
        {
            /* .Value --> The lookup method can return either an integer or a null
                 * if it returns an integer, we would be able to assing that to priorityId, which is an int type
                 * However if it returns a null we will have trouble. That why we extend that and use .Value 
                 * to tackle that issue
                 */
            int priorityId = (await LookupTicketPriorityIdAsync(priorityName)).Value;
            try
            {   
                
                List<Ticket> tickets = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                     .Include(t => t.Attachments)
                                                     .Include(t => t.Comments)
                                                     .Include(t => t.DeveloperUser)
                                                     .Include(t => t.History)
                                                     .Include(t => t.OwnerUser)
                                                     .Include(t => t.TicketPriority)
                                                     .Include(t => t.TicketStatus)
                                                     .Include(t => t.Project)
                                                  .Where(t => t.TicketPriorityId == priorityId)
                                                     .ToListAsync();
                return tickets;

               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetAllTicketsByStatusAsync(int companyId, string statusName)
        {
            int statusId = (await LookupTicketStatusIdAsync(statusName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                     .Include(t => t.Attachments)
                                                     .Include(t => t.Comments)
                                                     .Include(t => t.DeveloperUser)
                                                     .Include(t => t.History)
                                                     .Include(t => t.OwnerUser)
                                                     .Include(t => t.TicketPriority)
                                                     .Include(t => t.TicketStatus)
                                                     .Include(t => t.Project)
                                                     .Where(t => t.TicketStatusId == statusId)
                                                     .ToListAsync();
                return tickets;


            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<List<Ticket>> GetAllTicketsByTypeAsync(int companyId, string typeName)
        {
            int typeId = (await LookupTicketTypeIdAsync(typeName)).Value;

            try
            {
                List<Ticket> tickets = await _context.Projects.Where(p => p.CompanyId == companyId)
                                                     .SelectMany(p => p.Tickets)
                                                     .Include(t => t.Attachments)
                                                     .Include(t => t.Comments)
                                                     .Include(t => t.DeveloperUser)
                                                     .Include(t => t.History)
                                                     .Include(t => t.OwnerUser)
                                                     .Include(t => t.TicketPriority)
                                                     .Include(t => t.TicketStatus)
                                                     .Include(t => t.Project)
                                                     .Where(t => t.TicketTypeId == typeId)
                                                     .ToListAsync();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetArchivedTicketsAsync(int companyId)
        {
            try
            {
                List<Ticket> tickets = (await GetAllTicketsByCompanyAsync(companyId))
                                                         
                                                         .Where(t => t.Archived == true)
                                                         .ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByPriorityAsync(string priorityName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();
            try
            {
                tickets = (await GetAllTicketsByPriorityAsync(companyId, priorityName))
                                    .Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }


        // This will give us all the tickets as a developer for a particular project. The role can be even a submitter
        public async Task<List<Ticket>> GetProjectTicketsByRoleAsync(string role, string userId, int projectId, int companyId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetTicketsByRoleAsync(role, userId, companyId))
                            .Where(t => t.ProjectId == projectId)
                            .ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByStatusAsync(string statusName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();
            try
            {
                tickets = (await GetAllTicketsByStatusAsync(companyId, statusName))
                                .Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetProjectTicketsByTypeAsync(string typeName, int companyId, int projectId)
        {
            List<Ticket> tickets = new();

            try
            {
                tickets = (await GetAllTicketsByTypeAsync(companyId, typeName))
                    .Where(t => t.ProjectId == projectId).ToList();
                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IssueHubUser> GetTicketDeveloperAsync(int ticketId, int companyId)
        {
            IssueHubUser developer = new();
            try { 
            
               Ticket ticket = (await GetAllTicketsByCompanyAsync(companyId))
                    .FirstOrDefault(t => t.Id == ticketId);

                // if ticket is not equal to null 
                if(ticket?.DeveloperUserId != null)
                {
                    developer = ticket.DeveloperUser; // we are setting it to the object, not the Id
                    
                }
                return developer;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByRoleAsync(string role, string userId, int companyId)
        {
            List<Ticket> tickets = new();
            try
            {
                if(role == Roles.Admin.ToString())
                {
                    await GetAllTicketsByCompanyAsync(companyId);
                }
                else if(role == Roles.Developer.ToString())
                {
                    (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.DeveloperUserId == userId)
                                                                  .ToList();
                }
                else if (role == Roles.Submitter.ToString())
                {
                    /*Submitter becomes the owner of the ticket i.e who initializes the ticket

                      Submitters can submit the tickets, edit the tickets, 
                      be part of a project where they're managing the tickets

                    */

                    (await GetAllTicketsByCompanyAsync(companyId)).Where(t => t.OwnerUserId == userId)
                                                                  .ToList();
                }
                else if (role == Roles.ProjectManager.ToString())
                {
                    tickets = await GetTicketsByUserIdAsync(userId, companyId); 

                }

                else if (role == Roles.Admin.ToString())
                {

                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(string userId, int companyId)
        {
            IssueHubUser issueHubUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            List<Ticket> tickets = new();
            try
            {
                if (await _rolesService.IsUserInRoleAsync(issueHubUser, Roles.Admin.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                       .SelectMany(p => p.Tickets)
                       .ToList();

                }
                else if (await _rolesService.IsUserInRoleAsync(issueHubUser, Roles.Developer.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                       .SelectMany(p => p.Tickets)
                       .Where(t => t.DeveloperUserId == userId)
                       .ToList();
                }
                else if (await _rolesService.IsUserInRoleAsync(issueHubUser, Roles.Submitter.ToString()))
                {
                    tickets = (await _projectService.GetAllProjectsByCompany(companyId))
                      .SelectMany(p => p.Tickets)
                      .Where(t => t.OwnerUserId == userId)
                      .ToList();
                }

                else if (await _rolesService.IsUserInRoleAsync(issueHubUser, Roles.ProjectManager.ToString()))
                { 
                    // Project Manager doesn't have an Id attached to a Project or to a Ticket
                    // Project manager is based on the project they are assigned to
                    tickets = (await _projectService.GetUserProjectsAsync(userId))
                      .SelectMany(p => p.Tickets)
                      .ToList();
                }

                return tickets;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // All these lookup methods are helper methods that will give us the Ids when we provide various input

        // The nullable int return value means we may find the Id, we may NOT
        public async Task<int?> LookupTicketPriorityIdAsync(string priorityName)
        {
            try
            {
                TicketPriority priority = await _context.TicketPriorities.FirstOrDefaultAsync(p => p.Name == priorityName);
                return priority?.Id;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketStatusIdAsync(string statusName)
        {
            try
            {
                TicketStatus status = await _context.TicketStatuses.FirstOrDefaultAsync(s => s.Name == statusName);
                return status?.Id;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<int?> LookupTicketTypeIdAsync(string typeName)
        {
            try
            {
                TicketType type = await _context.TicketType.FirstOrDefaultAsync(type => type.Name == typeName);
                return type?.Id;
            }
            catch(Exception)
            {
                throw;
            }
        }


        // CRUD --> Update/Edit Ticket
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
