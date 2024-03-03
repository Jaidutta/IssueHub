using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace IssueHub.Services
{
    public class IssueHubRolesService : IIssueHubRolesService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IssueHubUser> _userManager;
        public IssueHubRolesService(ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<IssueHubUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Returns a bool to confirm if the user has been added to the role
        public async Task<bool> AddUserToRoleAsync(IssueHubUser user, string roleName)
        {
            bool result = (await _userManager.AddToRoleAsync(user, roleName)).Succeeded;

            return result;
        }

        // Returns a string of the role name for display
        public async Task<string> GetRoleNameByIdAsync(string roleId)
        {                         // Because we have the id, we can use Find
            IdentityRole role =  _context.Roles.Find(roleId);
            string result = await _roleManager.GetRoleNameAsync(role);

            return result;
        }

        // returns the List<string> user roles
        public async Task<IEnumerable<string>> GetUserRolesAsync(IssueHubUser user)
        {
            IEnumerable<string> result = await _userManager.GetRolesAsync(user);
            return result;
        }

        // Returns a list of all users in a role, filtered (queried) by company 
        public async Task<List<IssueHubUser>> GetUsersInRoleAsync(string roleName, int companyId)
        {
            List<IssueHubUser> users = (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
            List<IssueHubUser> result = users.Where(u => u.CompanyId == companyId).ToList();

            return result;
        }

        public async Task<List<IssueHubUser>> GetUsersNotInRoleAsync(string roleName, int companyId)
        {      
            // userIds are Guids --> alphanumeric characters and some dashes
                                    //Get the users with that role and select only the Id column
            List<string> userIds = (await _userManager.GetUsersInRoleAsync(roleName)).Select(u => u.Id).ToList();
            List<IssueHubUser> roleUsers =  _context.Users.Where(u => !userIds.Contains(u.Id)).ToList();

            List<IssueHubUser> result = roleUsers.Where(u => u.CompanyId == companyId).ToList();

            return result;
        }

        // checks if the user
        public async Task<bool> IsUserInRoleAsync(IssueHubUser user, string roleName)
        {
            bool result = await _userManager.IsInRoleAsync(user, roleName);
            return result;
        }

        public async Task<bool> RemoveUserFromRoleAsync(IssueHubUser user, string roleName)
        {
            var result = (await _userManager.RemoveFromRoleAsync(user, roleName)).Succeeded; 
            return result;

        }

        public async Task<bool> RemoveUserFromRolesAsync(IssueHubUser user, IEnumerable<string> roles)
        {
            var result = (await _userManager.RemoveFromRolesAsync(user, roles)).Succeeded;
            return result;
        }
    }
}
