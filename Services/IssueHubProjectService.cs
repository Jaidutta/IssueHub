using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using IssueHub.Models.Enums;
using System.Diagnostics.Metrics;
using Humanizer;

namespace IssueHub.Services
{
    public class IssueHubProjectService : IIssueHubProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIssueHubRolesService _rolesService;
        

        public IssueHubProjectService(ApplicationDbContext context, IIssueHubRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
            
        }

        // CRUD --> Create Project
        public async Task AddNewProjectAsync(Project project)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AddProjectManagerAsync(string userId, int projectId)
        {
            IssueHubUser pmManager = await GetProjectManagerAsync(projectId);

            // If there is a PM, remove the current PM
            if (pmManager != null)
            {
                try
                {
                    await RemoveProjectManagerAsync(projectId);
                }
                catch(Exception ex)
                {
                    Console.Write("$ Error Removing current Project Manager.{ex.Message}");
                    return false;
                }
            }

            // Add a PM
            try
            {
                await AddUserToProjectAsync(userId, projectId);
                return true;
            }
            catch(Exception ex)
            {
                Console.Write($"Error Adding new PM. {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddUserToProjectAsync(string userId, int projectId)
        {
            IssueHubUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(user != null)
            {
                Project project = (await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId));

                if(!await IsUserOnProjectAsync(userId, projectId) && project != null)
                {
                    try
                    {
                        project.Members.Add(user);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch(Exception)
                    {
                        throw;

                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        // CRUD --> Archive or "Delete" Project
        public async Task ArchiveProjectAsync(Project project)
        {
            project.Archived = true;
            _context.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task<List<IssueHubUser>> GetAllProjectMembersExceptPMAsync(int projectId)
        {
            List<IssueHubUser> developers = await GetProjectMembersByRoleAsync(projectId, Roles.Developer.ToString());
            List<IssueHubUser> submitters = await GetProjectMembersByRoleAsync(projectId, Roles.Submitter.ToString());
            List<IssueHubUser> admins = await GetProjectMembersByRoleAsync(projectId, Roles.Admin.ToString());
            
            List<IssueHubUser> teamMembers =  developers.Concat(submitters).Concat(admins).ToList();

            return teamMembers;

        }

        public async Task<List<Project>> GetAllProjectsByCompany(int companyId)
        {
            List<Project> projects = new();
            projects = await _context.Projects.Where(p => p.CompanyId == companyId && p.Archived == false)
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
            return projects;
        }

        public async Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName)
        {
            List<Project> projects = await GetAllProjectsByCompany(companyId);
            int priorityId = await LookupProjectPriorityId(priorityName);

            return projects.Where(p => p.ProjectPriorityId == priorityId).ToList();
        }

        public async Task<List<Project>> GetArchivedProjectsByCompany(int companyId)
        {
            List<Project> projects = await GetAllProjectsByCompany(companyId);
                
            return projects.Where(p => p.Archived == true).ToList();
        }

        public async Task<List<IssueHubUser>> GetDevelopersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        // CRUD -> Read
        public async Task<Project> GetProjectByIdAsync(int projectId, int companyId)
        {
            Project project = await _context.Projects
                                                .Include(p => p.Tickets)
                                                .Include(p => p.ProjectPriority)
                                                .Include(p => p.Members)
                                                .FirstOrDefaultAsync(p => p.Id == projectId 
                                                    && p.CompanyId == companyId);
            return project;

        }

        public async Task<IssueHubUser> GetProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);
            
            /* project? to prevent any runtime error, if FirstOrDefaultAsync returns a null
             * This is simply a check --> if the project is null do not execute the for loop
             */
            foreach(var member in project?.Members)
            {
                if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                {
                    return member;
                }
            }
            return null;
        }

        public async Task<List<IssueHubUser>> GetProjectMembersByRoleAsync(int projectId, string role)
        {
            Project project = await _context.Projects
                                            .Include(p => p.Members)
                                            .FirstOrDefaultAsync(p => p.Id == projectId);

            List <IssueHubUser> members = new();

            foreach(var user in project.Members)
            {
                if(await _rolesService.IsUserInRoleAsync(user, role))
                {
                    members.Add(user);
                }
            }

            return members;
        }

        public Task<List<IssueHubUser>> GetSubmittersOnProjectAsync(int projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetUserProjectsAsync(string userId)
        {
            try
            {
                List<Project> userProjects = (await _context.Users
               .Include(u => u.Projects)
                   .ThenInclude(p => p.Company)
               .Include(u => u.Projects)
                   .ThenInclude(p => p.Members)
               .Include(u => u.Projects)
                   .ThenInclude(p => p.Tickets)
               .Include(u => u.Projects)
                   .ThenInclude(t => t.Tickets)
                      .ThenInclude(t => t.DeveloperUser)
               .Include(u => u.Projects)
                   .ThenInclude(t => t.Tickets)
                      .ThenInclude(t => t.OwnerUser)
               .Include(u => u.Projects)
                   .ThenInclude(t => t.Tickets)
                      .ThenInclude(t => t.TicketPriority)
               .Include(u => u.Projects)
                   .ThenInclude(t => t.Tickets)
                      .ThenInclude(t => t.TicketStatus)
               .Include(u => u.Projects)
                   .ThenInclude(t => t.Tickets)
                      .ThenInclude(t => t.TicketType)
               .FirstOrDefaultAsync(u => u.Id == userId)).Projects.ToList();

                return userProjects;

            }
            catch (Exception ex)
            {
                Console.Write($"******** ERROR ********  <-- Error Getting User Project List --> {ex.Message}");
                throw;
            }
        }

        public async Task<List<IssueHubUser>> GetUsersNotOnProjectAsync(int projectId, int companyId)
        {
            // New Way of fetching info using .All
            List<IssueHubUser> users =  await _context.Users.Where(u => u.Projects.All(p => p.Id == projectId)).ToListAsync();

            return users.Where(u => u.CompanyId == companyId).ToList();
        }

        public async Task<bool>IsUserOnProjectAsync(string userId, int projectId)
        {
            Project project = await _context.Projects.Include(p => p.Members)
                                                .FirstOrDefaultAsync(p => p.Id == projectId);

            var result = false;
            if (project != null)
            {
               return project.Members.Any(m => m.Id == userId);
            }

            return result;
        }

        public async Task<int> LookupProjectPriorityId(string priorityName)
        {
            int priorityId = (await _context.ProjectPriorities.FirstOrDefaultAsync(pp => pp.Name == priorityName)).Id;
            return priorityId;
        }

        public async Task RemoveProjectManagerAsync(int projectId)
        {
            Project project = await _context.Projects.Include(p => p.Members)
                                                     .FirstOrDefaultAsync(p => p.Id == projectId);

            try
            {
                /* project ? to prevent any runtime error, if FirstOrDefaultAsync returns a null
                 * This is simply a check-- > if the project is null do not execute the for loop
                 */
                foreach (IssueHubUser member in project?.Members)
                {
                    if (await _rolesService.IsUserInRoleAsync(member, Roles.ProjectManager.ToString()))
                    {
                        await RemoveUserFromProjectAsync(member.Id, projectId);
                    }
                }    
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task RemoveUserFromProjectAsync(string userId, int projectId)
        {
            try
            {
                IssueHubUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                try
                {
                    if (await IsUserOnProjectAsync(userId, projectId))
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception) 
                {
                    throw;
                }

            }
            catch(Exception ex)
            {
                Console.Write($"******* ERROR *******  --> Error Removing User from Project {ex.Message}");
            }
        }

        public async Task RemoveUsersFromProjectByRoleAsync(string role, int projectId)
        {
            try
            {
                List<IssueHubUser> members = await GetProjectMembersByRoleAsync(projectId, role);
                Project project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                foreach(var user in  members)
                {
                    try
                    {
                        project.Members.Remove(user);
                        await _context.SaveChangesAsync();
                    }
                    catch( Exception ex ) 
                    {
                        throw;
                    }
                }

            }
            catch (Exception ex) 
            {
                Console.Write($"******** ERROR ******** Remove User from Project by Role--> {ex.Message}");
                throw;

            }
        }


        // CRUD --> Edit
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
