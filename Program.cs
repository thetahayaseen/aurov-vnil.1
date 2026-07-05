using mark_vnil._1;
using mark_vnil._1.Data;
using mark_vnil._1.Repositories;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// Inject DbContext & Repositories in the App as a Service

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite("Data Source=aurov.db")
);

builder.Services.AddScoped<ROVStreamRepository>();
builder.Services.AddScoped<ROVDetectedItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ROVStream}/{action=Dashboard}/{id?}")
    .WithStaticAssets();

app.MapHub<ROVStreamHub>("/rovstreamhub");
app.MapHub<ROVDetectedItemHub>("/rovdetecteditemhub");

app.Run();
