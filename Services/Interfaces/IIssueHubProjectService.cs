using IssueHub.Models;

namespace IssueHub.Services.Interfaces
{
    public interface IIssueHubProjectService
    {
        public Task AddNewProjectAsync(Project project);

        public Task<bool> AddProjectManagerAsync(string userId, int projectId);

        public Task<bool> AddUserToProjectAsync(string userId, int projectId);

        public Task ArchiveProjectAsync(Project project);

        public Task<List<Project>> GetAllProjectsByCompany(int companyId);

        public Task<List<Project>> GetAllProjectsByPriority(int companyId, string priorityName);

        public Task<List<IssueHubUser>> GetAllProjectMembersExceptPMAsync(int projectId);

        public Task<List<Project>> GetArchivedProjectsByCompany(int companyId);

        public Task<List<IssueHubUser>> GetDevelopersOnProjectAsync(int projectId);

        public Task<IssueHubUser> GetProjectManagerAsync(int projectId);

        public Task<List<IssueHubUser>> GetProjectMembersByRoleAsync(int projectId, string role);

        public Task<Project> GetProjectByIdAsync(int projectId, int companyId);

        public Task<List<IssueHubUser>> GetSubmittersOnProjectAsync(int projectId);

        public Task<List<IssueHubUser>> GetUsersNotOnProjectAsync(int projectId, int companyId);

        public Task<List<Project>> GetUserProjectsAsync(string userId);

        public Task<bool> IsUserOnProjectAsync(string userId, int projectId);

        public Task<int> LookupProjectPriorityId(string priorityName);

        public Task RemoveProjectManagerAsync(int projectId);

        public Task RemoveUsersFromProjectByRoleAsync(string role, int projectId);

        public Task RemoveUserFromProjectAsync(string userId, int projectId);

        public Task UpdateProjectAsync(Project project);
    }
}
