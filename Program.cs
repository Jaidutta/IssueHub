using IssueHub.Data;
using IssueHub.Models;
using IssueHub.Services;
using IssueHub.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IssueHubUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

///***********************************CUSTOM SERVICES********************************

// --- IssueHub Services ---
// Roles Services
builder.Services.AddScoped<IIssueHubRolesService, IssueHubRolesService>();

// CompanyInfo Services
builder.Services.AddScoped<IIssueHubCompanyInfoService, IssueHubCompanyInfoService>();

// Project Services
builder.Services.AddScoped<IIssueHubProjectService,  IssueHubProjectService>();

// Ticket Services
builder.Services.AddScoped<IIssueHubTicketService, IssueHubTicketService>();

// TicketHistory Services
builder.Services.AddScoped<IIssueHubTicketHistoryService, IssueHubTicketHistoryService>();

// Email Services
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IEmailSender, IssueHubEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
