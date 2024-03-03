using IssueHub.Models;

namespace IssueHub.Services.Interfaces
{
    public interface IIssueHubCompanyInfoService
    {
       public Task<Company> GetCompanyInfoByIdAsync(int? companyId);

        public Task<List<IssueHubUser>> GetAllMemberAsync(int companyId);

        public Task<List<Project>> GetAllProjectsAsync(int companyId);

        public Task<List <Ticket>> GetAllTicketsAsync(int companyId);
    }
}
