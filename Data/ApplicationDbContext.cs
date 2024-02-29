using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IssueHub.Models;

namespace IssueHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<IssueHubUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}