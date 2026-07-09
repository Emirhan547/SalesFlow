using Microsoft.AspNetCore.Identity;
using QuestPDF.Infrastructure;
using SalesFlow.API.Middlewares;
using SalesFlow.Business.Extensions;
using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Extensions;
using SalesFlow.DataAccess.Seed;
using SalesFlow.Entity.Entities;
using Scalar.AspNetCore;
QuestPDF.Settings.License = LicenseType.Community;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessServices(builder.Configuration);

builder.Services.AddBusinessServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseGlobalException();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
using (IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<AppRole> roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await RoleSeeder.SeedAsync(roleManager);
}
app.Run();