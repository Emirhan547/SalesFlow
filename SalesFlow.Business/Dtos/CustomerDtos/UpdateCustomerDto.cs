using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }

        public CustomerType CustomerType { get; set; }

        public string? CompanyName { get; set; }

        public string ContactFirstName { get; set; } = null!;

        public string ContactLastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string? Website { get; set; }

        public string? TaxNumber { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
