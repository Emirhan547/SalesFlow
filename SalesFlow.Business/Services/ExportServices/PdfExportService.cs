using QuestPDF.Fluent;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ExportServices
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] ExportCustomers(IEnumerable<Customer> customers)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("SalesFlow CRM - Customer Report")
                        .FontSize(20)
                        .Bold();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Id").Bold();
                                header.Cell().Text("Name").Bold();
                                header.Cell().Text("Company").Bold();
                                header.Cell().Text("Email").Bold();
                                header.Cell().Text("Phone").Bold();
                            });

                            foreach (Customer customer in customers)
                            {
                                table.Cell().Text(customer.Id);

                                table.Cell().Text($"{customer.ContactFirstName} {customer.ContactLastName}");

                                table.Cell().Text(customer.CompanyName ?? "-");

                                table.Cell().Text(customer.Email);

                                table.Cell().Text(customer.PhoneNumber);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span($"Generated: {DateTime.Now:dd.MM.yyyy HH:mm}");
                        });
                });
            }).GeneratePdf();
        }

        public byte[] ExportLeads(IEnumerable<Lead> leads)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("SalesFlow CRM - Lead Report")
                        .FontSize(20)
                        .Bold();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Id").Bold();
                                header.Cell().Text("Name").Bold();
                                header.Cell().Text("Company").Bold();
                                header.Cell().Text("Status").Bold();
                                header.Cell().Text("Source").Bold();
                                header.Cell().Text("Email").Bold();
                            });

                            foreach (Lead lead in leads)
                            {
                                table.Cell().Text(lead.Id);

                                table.Cell().Text($"{lead.FirstName} {lead.LastName}");

                                table.Cell().Text(lead.CompanyName ?? "-");

                                table.Cell().Text(lead.Status.ToString());

                                table.Cell().Text(lead.Source.ToString());

                                table.Cell().Text(lead.Email);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated : {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }

        public byte[] ExportDeals(IEnumerable<Deal> deals)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("SalesFlow CRM - Deal Report")
                        .FontSize(20)
                        .Bold();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Id").Bold();
                                header.Cell().Text("Title").Bold();
                                header.Cell().Text("Customer").Bold();
                                header.Cell().Text("Stage").Bold();
                                header.Cell().Text("Amount").Bold();
                            });

                            foreach (Deal deal in deals)
                            {
                                table.Cell().Text(deal.Id);

                                table.Cell().Text(deal.Title);

                                table.Cell().Text(
                                    $"{deal.Customer.ContactFirstName} {deal.Customer.ContactLastName}");

                                table.Cell().Text(deal.Stage.ToString());

                                table.Cell().Text($"{deal.Amount:N2}");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated : {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            }).GeneratePdf();
        }
    }
}