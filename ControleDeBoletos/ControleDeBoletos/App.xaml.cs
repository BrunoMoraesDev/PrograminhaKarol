using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ControleDeBoletos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        private string _connectionString = "Data Source=database.db; Foreign Keys=true";

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<ControleBoletosContext>(options =>
            {
                options.UseSqlite(_connectionString);
            });

            services.AddSingleton<MainWindow>();
            services.AddScoped<ControleBoletosContext>();
            services.AddScoped<BoletoRepository>();
            services.AddScoped<TipoBoletoRepository>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
