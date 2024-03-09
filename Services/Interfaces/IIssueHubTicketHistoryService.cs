using IssueHub.Models;

namespace IssueHub.Services.Interfaces
{
    public interface IIssueHubTicketHistoryService
    {
        Task AddHistoryAsync(Ticket oldTicket,  Ticket newTicket, string userId);

        Task<List<TicketHistory>> GetCompanyTicketsHistoriesAsync(int companyId);

        Task<List<TicketHistory>> GetProjectTicketsHistoriesAsync(int projectId, int companyId);

    }
}
