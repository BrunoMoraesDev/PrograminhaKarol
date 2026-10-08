using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Windows;

namespace ControleDeBoletos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public static string DatabasePath { get; } = ResolveDatabasePath();

        private static string ResolveDatabasePath()
        {
            // Honor the original database locations before choosing a writable default.
            string workingDatabase = Path.GetFullPath("database.db");
            if (File.Exists(workingDatabase)) return workingDatabase;

            string applicationDatabase = Path.Combine(AppContext.BaseDirectory, "database.db");
            if (File.Exists(applicationDatabase)) return applicationDatabase;

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ControleDeBoletos", "database.db");
        }

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
                options.UseSqlite(new SqliteConnectionStringBuilder
                {
                    DataSource = DatabasePath,
                    ForeignKeys = true
                }.ToString());
            });

            services.AddSingleton<MainWindow>();
            services.AddScoped<ControleBoletosContext>();
            services.AddScoped<BoletoRepository>();
            services.AddScoped<TipoBoletoRepository>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath)!);
            var context = serviceProvider.GetRequiredService<ControleBoletosContext>();
            context.Database.EnsureCreated();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            serviceProvider.Dispose();
            base.OnExit(e);
        }
    }
}
