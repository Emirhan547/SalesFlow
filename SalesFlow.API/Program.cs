using Microsoft.AspNetCore.Identity;
using SalesFlow.API.Middlewares;
using SalesFlow.Business.Extensions;
using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Extensions;
using SalesFlow.Entity.Entities;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
})

.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); builder.Services.AddControllers();

builder.Services.AddOpenApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.UseGlobalException();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

app.Run();
