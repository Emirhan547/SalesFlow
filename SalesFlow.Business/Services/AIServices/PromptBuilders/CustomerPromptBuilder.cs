using System.Text;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.AIServices.PromptBuilders
{
    public static class CustomerPromptBuilder
    {
        public static string Build(Customer customer)
        {
            StringBuilder sb = new();

            sb.AppendLine("=== CUSTOMER INFORMATION ===");
            sb.AppendLine();

            sb.AppendLine($"Company: {customer.CompanyName ?? "Individual Customer"}");
            sb.AppendLine($"Contact: {customer.ContactFirstName} {customer.ContactLastName}");
            sb.AppendLine($"Email: {customer.Email}");
            sb.AppendLine($"Phone: {customer.PhoneNumber}");
            sb.AppendLine($"Customer Type: {customer.CustomerType}");

            if (!string.IsNullOrWhiteSpace(customer.Website))
                sb.AppendLine($"Website: {customer.Website}");

            if (!string.IsNullOrWhiteSpace(customer.Address))
                sb.AppendLine($"Address: {customer.Address}");

            if (customer.AssignedUser is not null)
            {
                sb.AppendLine($"Assigned User: {customer.AssignedUser.FirstName} {customer.AssignedUser.LastName}");
            }

            sb.AppendLine();

            // DEALS
            sb.AppendLine("=== DEALS ===");

            if (customer.Deals.Any())
            {
                foreach (Deal deal in customer.Deals)
                {
                    sb.AppendLine($"Title: {deal.Title}");
                    sb.AppendLine($"Stage: {deal.Stage}");
                    sb.AppendLine($"Amount: {deal.Amount:C}");

                    if (deal.ExpectedCloseDate.HasValue)
                        sb.AppendLine($"Expected Close: {deal.ExpectedCloseDate:dd.MM.yyyy}");

                    if (!string.IsNullOrWhiteSpace(deal.Description))
                        sb.AppendLine($"Description: {deal.Description}");

                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No deals found.");
            }

            // MEETINGS
            sb.AppendLine("=== MEETINGS ===");

            if (customer.Meetings.Any())
            {
                foreach (Meeting meeting in customer.Meetings.OrderByDescending(x => x.StartDate))
                {
                    sb.AppendLine($"Title: {meeting.Title}");
                    sb.AppendLine($"Type: {meeting.Type}");
                    sb.AppendLine($"Status: {meeting.Status}");
                    sb.AppendLine($"Start: {meeting.StartDate:dd.MM.yyyy HH:mm}");

                    if (!string.IsNullOrWhiteSpace(meeting.Description))
                        sb.AppendLine($"Description: {meeting.Description}");

                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No meetings found.");
            }

            // TASKS
            sb.AppendLine("=== TASKS ===");

            if (customer.TaskItems.Any())
            {
                foreach (TaskItem task in customer.TaskItems.OrderBy(x => x.DueDate))
                {
                    sb.AppendLine($"Title: {task.Title}");
                    sb.AppendLine($"Status: {task.Status}");
                    sb.AppendLine($"Priority: {task.Priority}");
                    sb.AppendLine($"Due Date: {task.DueDate:dd.MM.yyyy}");

                    if (!string.IsNullOrWhiteSpace(task.Description))
                        sb.AppendLine($"Description: {task.Description}");

                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No tasks found.");
            }

            // NOTES
            sb.AppendLine("=== NOTES ===");

            if (customer.Notes.Any())
            {
                foreach (Note note in customer.Notes.OrderByDescending(x => x.CreatedDate))
                {
                    sb.AppendLine($"Date: {note.CreatedDate:dd.MM.yyyy}");

                    sb.AppendLine($"Note: {note.Content}");

                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No notes found.");
            }

            return sb.ToString();
        }
    }
}