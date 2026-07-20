using SalesFlow.Business.Dtos.CustomerDtos;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System.Text;
using TaskStatus = SalesFlow.Entity.Enums.TaskStatus;

namespace SalesFlow.Business.Services.AIServices.PromptBuilders;

public static class FollowUpEmailPromptBuilder
{
    public static string Build(
        Customer customer,
        GenerateFollowUpEmailDto dto)
    {
        StringBuilder sb = new();

        sb.AppendLine("=== CUSTOMER INFORMATION ===");
        sb.AppendLine($"Company: {customer.CompanyName}");
        sb.AppendLine($"Contact: {customer.ContactFirstName} {customer.ContactLastName}");
        sb.AppendLine($"Email: {customer.Email}");
        sb.AppendLine($"Phone: {customer.PhoneNumber}");

        if (!string.IsNullOrWhiteSpace(customer.Website))
            sb.AppendLine($"Website: {customer.Website}");

        if (!string.IsNullOrWhiteSpace(customer.Address))
            sb.AppendLine($"Address: {customer.Address}");

        if (!string.IsNullOrWhiteSpace(customer.Description))
            sb.AppendLine($"Description: {customer.Description}");

        if (customer.AssignedUser is not null)
            sb.AppendLine($"Account Manager: {customer.AssignedUser.FirstName} {customer.AssignedUser.LastName}");

        sb.AppendLine();

        sb.AppendLine("=== EMAIL SETTINGS ===");
        sb.AppendLine($"Purpose: {dto.Purpose}");
        sb.AppendLine($"Tone: {dto.Tone}");

        sb.AppendLine();

        sb.AppendLine("=== OPEN DEALS ===");

        foreach (Deal deal in customer.Deals
                     .Where(x => x.Stage != DealStage.Won &&
                                 x.Stage != DealStage.Lost)
                     .OrderByDescending(x => x.Amount)
                     .Take(5))
        {
            sb.AppendLine($"Title: {deal.Title}");
            sb.AppendLine($"Stage: {deal.Stage}");
            sb.AppendLine($"Amount: {deal.Amount:C}");

            if (deal.ExpectedCloseDate.HasValue)
            {
                sb.AppendLine($"Expected Close Date: {deal.ExpectedCloseDate:dd.MM.yyyy}");
            }

            if (!string.IsNullOrWhiteSpace(deal.Description))
            {
                sb.AppendLine($"Description: {deal.Description}");
            }

            if (deal.AssignedUser is not null)
            {
                sb.AppendLine(
                    $"Owner: {deal.AssignedUser.FirstName} {deal.AssignedUser.LastName}");
            }

            sb.AppendLine();
        }

        sb.AppendLine("=== RECENT MEETINGS ===");

        foreach (Meeting meeting in customer.Meetings
                     .OrderByDescending(x => x.StartDate)
                     .Take(5))
        {
            sb.AppendLine($"{meeting.Title}");
            sb.AppendLine($"{meeting.StartDate:g}");
            sb.AppendLine($"{meeting.Status}");
            sb.AppendLine();
        }

        sb.AppendLine("=== OPEN TASKS ===");

        foreach (TaskItem task in customer.TaskItems
              .Where(x => x.Status != TaskStatus.Completed)
              .OrderBy(x => x.DueDate)
              .Take(5))
        {
            sb.AppendLine($"Title: {task.Title}");
            sb.AppendLine($"Priority: {task.Priority}");
            sb.AppendLine($"Status: {task.Status}");
            sb.AppendLine($"Due Date: {task.DueDate:dd.MM.yyyy}");
            sb.AppendLine();
        }

        sb.AppendLine("=== RECENT NOTES ===");

        foreach (Note note in customer.Notes
                     .OrderByDescending(x => x.CreatedDate)
                     .Take(5))
        {
            sb.AppendLine(note.Content);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}