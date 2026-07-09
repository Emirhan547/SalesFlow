using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.CustomerDtos
{
    public class CreateCustomerDto
    {
        public CustomerType CustomerType { get; set; }

        public string CompanyName { get; set; }

        public string ContactFirstName { get; set; } 

        public string ContactLastName { get; set; } 

        public string Email { get; set; } 

        public string PhoneNumber { get; set; }

        public string Website { get; set; }

        public string TaxNumber { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
        public int? AssignedUserId { get; set; }
    }
}
