using HCI.Models.Attractions.Repository;
using HCI.Models.Attractions.Service;
using HCI.Models.Locations.Repository;
using HCI.Models.Locations.Service;
using HCI.Models.Restaurants.Repository;
using HCI.Models.Restaurants.Service;
using HCI.Models.Restaurants.Service.HCI.Models.Restaurants.Service;
using HCI.Models.Users.Repository;
using HCI.Models.Users.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace HCI
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<AppDbContext>();
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<IUserRepository, UserRepository>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IRestaurantRepository, RestaurantRepository>();
                    services.AddTransient<IRestaurantService, RestaurantService>();
                    services.AddTransient<IAttractionRepository, AttractionRepository>();
                    services.AddTransient<IAttractionService, AttractionService>();
                    services.AddTransient<ILocationRepository, LocationRepository>();
                    services.AddTransient<ILocationService, LocationService>();

                }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var dbContext = AppHost.Services.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();

            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
