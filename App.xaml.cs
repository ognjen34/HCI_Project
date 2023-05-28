using HCI.Models.Accommodations.Repository;
using HCI.Models.Accommodations.Service;
using HCI.Models.Trips.Repository;
using HCI.Models.Trips.Service;
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
                    services.AddTransient<IAccommodationRepository, AccommodationRepository>();
                    services.AddTransient<IAccommodationService, AccommodationService>();
                    services.AddTransient<ITripRepository, TripRepository>();
                    services.AddTransient<ITripService, TripService>();
                    services.AddTransient<IOrderedTripRepository, OrderedTripRepository>();
                    services.AddTransient<IOrderedTripService, OrderedTripService>();
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
