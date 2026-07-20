using System.Text;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.AIServices.PromptBuilders;

public static class LeadSummaryPromptBuilder
{
    public static string Build(Lead lead)
    {
        StringBuilder sb = new();

        sb.AppendLine("=== LEAD INFORMATION ===");
        sb.AppendLine();

        sb.AppendLine($"Company: {lead.CompanyName ?? "Not Specified"}");
        sb.AppendLine($"Contact: {lead.FirstName} {lead.LastName}");
        sb.AppendLine($"Email: {lead.Email}");
        sb.AppendLine($"Phone: {lead.PhoneNumber}");
        sb.AppendLine($"Website: {lead.Website ?? "Not Specified"}");
        sb.AppendLine($"Address: {lead.Address ?? "Not Specified"}");
        sb.AppendLine($"Status: {lead.Status}");
        sb.AppendLine($"Source: {lead.Source}");
        sb.AppendLine(
            $"Assigned User: {(lead.AssignedUser is null ? "Not Assigned" : $"{lead.AssignedUser.FirstName} {lead.AssignedUser.LastName}")}"); sb.AppendLine($"Created Date: {lead.CreatedDate:dd.MM.yyyy HH:mm}");

        sb.AppendLine();
        sb.AppendLine("=== DESCRIPTION ===");
        sb.AppendLine(lead.Description ?? "No description provided.");

        return sb.ToString();
    }
}