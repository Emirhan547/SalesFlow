using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("SalesFlow CRM")
                            .FontSize(22)
                            .Bold();

                        column.Item()
                            .PaddingTop(4)
                            .Text("Customer Report")
                            .FontSize(15)
                            .SemiBold();

                        column.Item()
                            .PaddingTop(2)
                            .Text($"Generated: {DateTime.Now:dd.MM.yyyy HH:mm}    Total Customers: {customers.Count()}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item()
                            .PaddingTop(8)
                            .LineHorizontal(1);
                    });

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("#").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Name").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Company").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Email").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Phone").FontColor(Colors.White).Bold();
                            });

                            int index = 1;

                            foreach (Customer customer in customers)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(index++.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text($"{customer.ContactFirstName} {customer.ContactLastName}");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(customer.CompanyName ?? "-");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(customer.Email);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(customer.PhoneNumber);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("SalesFlow CRM • ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
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

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("SalesFlow CRM")
                            .FontSize(22)
                            .Bold();

                        column.Item()
                            .PaddingTop(4)
                            .Text("Lead Report")
                            .FontSize(15)
                            .SemiBold();

                        column.Item()
                            .PaddingTop(2)
                            .Text($"Generated: {DateTime.Now:dd.MM.yyyy HH:mm}    Total Leads: {leads.Count()}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item()
                            .PaddingTop(8)
                            .LineHorizontal(1);
                    });

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("#").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Name").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Company").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Status").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Source").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Email").FontColor(Colors.White).Bold();
                            });

                            int index = 1;

                            foreach (Lead lead in leads)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(index++.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text($"{lead.FirstName} {lead.LastName}");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(lead.CompanyName ?? "-");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(lead.Status.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(lead.Source.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(lead.Email);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("SalesFlow CRM • ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
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

                    page.Header().Column(column =>
                    {
                        column.Item()
                            .Text("SalesFlow CRM")
                            .FontSize(22)
                            .Bold();

                        column.Item()
                            .PaddingTop(4)
                            .Text("Deal Report")
                            .FontSize(15)
                            .SemiBold();

                        column.Item()
                            .PaddingTop(2)
                            .Text($"Generated: {DateTime.Now:dd.MM.yyyy HH:mm}    Total Deals: {deals.Count()}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken2);

                        column.Item()
                            .PaddingTop(8)
                            .LineHorizontal(1);
                    });

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(35);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("#").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Title").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Customer").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Stage").FontColor(Colors.White).Bold();
                                header.Cell().Background(Colors.Blue.Medium).Padding(6).Text("Amount").FontColor(Colors.White).Bold();
                            });

                            int index = 1;

                            foreach (Deal deal in deals)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(index++.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(deal.Title);

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text($"{deal.Customer.ContactFirstName} {deal.Customer.ContactLastName}");

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text(deal.Stage.ToString());

                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                    .Text($"{deal.Amount:N2}");
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("SalesFlow CRM • ");
                            text.CurrentPageNumber();
                            text.Span(" / ");
                            text.TotalPages();
                        });
                });
            }).GeneratePdf();
        }
    }
}