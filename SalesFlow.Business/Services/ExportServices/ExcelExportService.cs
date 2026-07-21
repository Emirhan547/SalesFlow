using ClosedXML.Excel;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalesFlow.Business.Services.ExportServices
{
    public class ExcelExportService : IExcelExportService
    {
        public byte[] ExportCustomers(IEnumerable<Customer> customers)
        {
            XLWorkbook workbook = CreateWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Customers");

            CreateCustomerHeader(worksheet);

            int row = 2;
            int index = 1;

            foreach (Customer customer in customers)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = customer.ContactFirstName;
                worksheet.Cell(row, 3).Value = customer.ContactLastName;
                worksheet.Cell(row, 4).Value = customer.CompanyName;
                worksheet.Cell(row, 5).Value = customer.Email;
                worksheet.Cell(row, 6).Value = customer.PhoneNumber;
                worksheet.Cell(row, 7).Value = customer.CustomerType.ToString();
                worksheet.Cell(row, 8).Value = customer.CreatedDate;
                worksheet.Cell(row, 8).Style.DateFormat.Format = "dd.MM.yyyy";

                row++;
            }

            return SaveWorkbook(workbook);
        }

        private static XLWorkbook CreateWorkbook()
        {
            return new XLWorkbook();
        }

        private static byte[] SaveWorkbook(XLWorkbook workbook)
        {
            foreach (IXLWorksheet worksheet in workbook.Worksheets)
            {
                worksheet.Columns().AdjustToContents();
            }

            using MemoryStream stream = new();

            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static void CreateCustomerHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Company";
            worksheet.Cell(1, 5).Value = "Email";
            worksheet.Cell(1, 6).Value = "Phone";
            worksheet.Cell(1, 7).Value = "Customer Type";
            worksheet.Cell(1, 8).Value = "Created Date";

            IXLRange header = worksheet.Range(1, 1, 1, 8);

            header.Style.Font.Bold = true;
            header.Style.Font.FontSize = 12;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.RangeUsed().SetAutoFilter();

            worksheet.SheetView.FreezeRows(1);
        }

        public byte[] ExportLeads(IEnumerable<Lead> leads)
        {
            XLWorkbook workbook = CreateWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Leads");

            CreateLeadHeader(worksheet);

            int row = 2;
            int index = 1;

            foreach (Lead lead in leads)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = lead.FirstName;
                worksheet.Cell(row, 3).Value = lead.LastName;
                worksheet.Cell(row, 4).Value = lead.CompanyName;
                worksheet.Cell(row, 5).Value = lead.Email;
                worksheet.Cell(row, 6).Value = lead.PhoneNumber;
                worksheet.Cell(row, 7).Value = lead.Status.ToString();
                worksheet.Cell(row, 8).Value = lead.Source.ToString();
                worksheet.Cell(row, 9).Value = lead.CreatedDate;
                worksheet.Cell(row, 9).Style.DateFormat.Format = "dd.MM.yyyy";

                row++;
            }

            return SaveWorkbook(workbook);
        }

        private static void CreateLeadHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "First Name";
            worksheet.Cell(1, 3).Value = "Last Name";
            worksheet.Cell(1, 4).Value = "Company";
            worksheet.Cell(1, 5).Value = "Email";
            worksheet.Cell(1, 6).Value = "Phone";
            worksheet.Cell(1, 7).Value = "Status";
            worksheet.Cell(1, 8).Value = "Source";
            worksheet.Cell(1, 9).Value = "Created Date";

            IXLRange header = worksheet.Range(1, 1, 1, 9);

            header.Style.Font.Bold = true;
            header.Style.Font.FontSize = 12;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.RangeUsed().SetAutoFilter();
            worksheet.SheetView.FreezeRows(1);
        }

        public byte[] ExportDeals(IEnumerable<Deal> deals)
        {
            XLWorkbook workbook = CreateWorkbook();

            IXLWorksheet worksheet = workbook.Worksheets.Add("Deals");

            CreateDealHeader(worksheet);

            int row = 2;
            int index = 1;

            foreach (Deal deal in deals)
            {
                worksheet.Cell(row, 1).Value = index++;
                worksheet.Cell(row, 2).Value = deal.Title;
                worksheet.Cell(row, 3).Value = deal.Customer.ContactFirstName + " " + deal.Customer.ContactLastName;
                worksheet.Cell(row, 4).Value = deal.Stage.ToString();
                worksheet.Cell(row, 5).Value = deal.Amount;
                worksheet.Cell(row, 6).Value = deal.CreatedDate;
                worksheet.Cell(row, 6).Style.DateFormat.Format = "dd.MM.yyyy";

                row++;
            }

            return SaveWorkbook(workbook);
        }

        private static void CreateDealHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, 1).Value = "#";
            worksheet.Cell(1, 2).Value = "Title";
            worksheet.Cell(1, 3).Value = "Customer";
            worksheet.Cell(1, 4).Value = "Stage";
            worksheet.Cell(1, 5).Value = "Amount";
            worksheet.Cell(1, 6).Value = "Created Date";

            IXLRange header = worksheet.Range(1, 1, 1, 6);

            header.Style.Font.Bold = true;
            header.Style.Font.FontSize = 12;
            header.Style.Fill.BackgroundColor = XLColor.LightGray;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.RangeUsed().SetAutoFilter();
            worksheet.SheetView.FreezeRows(1);
        }
    }
}