using Microsoft.AspNetCore.Identity;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<AppRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "SalesManager",
                "SalesRepresentative"
            };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole
                    {
                        Name = role
                    });
                }
            }
        }
    }
}