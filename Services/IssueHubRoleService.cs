using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IssueHub.Services
{
    public class IssueHubRoleService : IIssueHubRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IssueHubUser> _userManager;
        public IssueHubRoleService(ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<IssueHubUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> AddUserToRoleAsync(IssueHubUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;

            return result;
        }

        public Task<string> GetRoleNameByIdAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetUserRolesAsync(IssueHubUser user)
        {
            throw new NotImplementedException();
        }

        public Task<List<IssueHubUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<IssueHubUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUserInRoleAsync(IssueHubUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRoleAsync(IssueHubUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveUserFromRolesAsync(IssueHubUser user, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }
    }
}
