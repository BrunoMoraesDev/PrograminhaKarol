using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show($"Erro não tratado:\n\n{e.ExceptionObject}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            };

            DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show($"Erro de interface:\n\n{e.Exception.Message}", "Erro WPF", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
                Current.Shutdown();
            };

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
