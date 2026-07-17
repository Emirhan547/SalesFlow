using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuestPDF.Infrastructure;
using SalesFlow.API.Hubs;
using SalesFlow.API.Middlewares;
using SalesFlow.API.Services;
using SalesFlow.Business.Extensions;
using SalesFlow.Business.Services.RealtimeServices;
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
builder.Services.AddSignalR();
builder.Services.AddSingleton<
    IUserIdProvider,
    SignalRUserIdProvider>();
builder.Services.AddScoped<
    IRealtimeService,
    SignalRRealtimeService>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("React", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("React");
app.UseStaticFiles();
app.UseGlobalException();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SalesFlowHub>(
    "/hubs/salesflow");
using (IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<AppRole> roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

    await RoleSeeder.SeedAsync(roleManager);
}
app.Run();