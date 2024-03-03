using IssueHub.Models;

namespace IssueHub.Services
{
    public interface IIssueHubRolesService
    {
        // User Role Status, Add, Remove
        public Task<bool> IsUserInRoleAsync(IssueHubUser user, string roleName);

        public Task<IEnumerable<string>> GetUserRolesAsync(IssueHubUser user);

        public Task<bool> AddUserToRoleAsync(IssueHubUser user, string roleName);

        
        // Remove user from a single role
        public Task<bool> RemoveUserFromRoleAsync(IssueHubUser user, string roleName);


        // Remove user from a list of roles
        public Task<bool> RemoveUserFromRolesAsync(IssueHubUser user, IEnumerable<string> roles);


        // Role Users Information

        // Get Users(such as all developers, all PMs, etc in the company) 
        public Task<List<IssueHubUser>> GetUsersInRoleAsync(string roleName, int companyId);


        // Get Users NOT in a role (such as all developers, all PMs, etc in the company) 
        public Task<List<IssueHubUser>> GetUsersNotInRoleAsync(string roleName, int companyId);


        // Returns Role Name when we pass it the Id
        public Task<string> GetRoleNameByIdAsync(string roleId);
    }
}
