using Data.DataContext;
using Data.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
         

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AirlineDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AirlineDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AirlineDbContext>(options => options.UseSqlServer(connectionString));
            //builder.Services.AddDbContext<AirlineDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyConnectionString")));



            //to use the data folder that contains json data files we need to create a path to the file

            string pathToFlightJsonFile = builder.Environment.ContentRootPath + "Data\\" + "flights.json";
            string pathToTicketJsonFile = builder.Environment.ContentRootPath + "Data\\" + "tickets.json";

            //json flight repository

            builder.Services.AddScoped<IFlights, FlightsJsonRepository>(x => new FlightsJsonRepository(pathToFlightJsonFile));
            builder.Services.AddScoped<ITickets, TicketJsonRepository>(x => new TicketJsonRepository(pathToTicketJsonFile));

            //sql flight repository
            builder.Services.AddScoped(typeof(FlightDbRepository));
            builder.Services.AddScoped(typeof(TicketDbRepository));
            

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
        }
    }
}