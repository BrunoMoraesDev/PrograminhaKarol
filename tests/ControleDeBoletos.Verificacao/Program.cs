using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ControleDeBoletos;
using ControleDeBoletos.Controls;
using ControleDeBoletos.Data.DbContexts;
using ControleDeBoletos.Data.Repositories;
using ControleDeBoletos.Enums;
using ControleDeBoletos.Models;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    private static void Check(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    [STAThread]
    private static void Main()
    {
        string output = Path.GetFullPath("artifacts/verificacao");
        Directory.CreateDirectory(output);
        string database = Path.Combine(output, $"teste-{Guid.NewGuid():N}.db");
        var options = new DbContextOptionsBuilder<ControleBoletosContext>()
            .UseSqlite($"Data Source={database};Foreign Keys=true").Options;
        using var context = new ControleBoletosContext(options);
        Check(context.Database.EnsureCreated(), "O banco novo deve ser criado.");
        var categories = new TipoBoletoRepository(context);
        var boletos = new BoletoRepository(context);
        var category = categories.Add(new TipoBoleto { Descricao = "Moradia" });
        var boleto = boletos.Incluir(new Boleto
        {
            Descricao = "Boleto de verificação", TipoId = category.Id, Valor = 123.45m,
            Emissao = DateTime.Today, Vencimento = DateTime.Today.AddDays(5)
        });
        using (var reopened = new ControleBoletosContext(options))
        {
            Check(!reopened.Database.EnsureCreated(), "Não deve recriar banco existente.");
            Check(reopened.Boleto.Single().Valor == 123.45m, "Dados devem persistir.");
        }
        Check(boletos.BuscarBoletosFiltrados(null, null, TiposSituacao.PENDENTES,
            null, null, null, null, null, null).Count() == 1, "Consulta SQLite falhou.");

        var app = new Application();
        app.Resources.MergedDictionaries.Add(new HandyControl.Themes.Theme());
        var window = new MainWindow(boletos, categories);
        var tabs = (TabControl)window.FindName("tabControlMenu");
        // Exercise real templates and layout without opening a desktop window.
        var root = (FrameworkElement)window.Content;
        window.Content = null;
        var host = new Border { Child = root, Background = Brushes.White };
        var sizes = new[] { new Size(480, 420), new Size(640, 480), new Size(800, 600),
            new Size(1024, 768), new Size(1366, 768), new Size(1920, 1080) };
        foreach (var size in sizes)
        {
            for (int tab = 0; tab < tabs.Items.Count; tab++)
            {
                tabs.SelectedIndex = tab;
                host.Width = size.Width;
                host.Height = size.Height;
                Layout(host, size);
                if (tab == 0)
                {
                    var description = (FrameworkElement)window.FindName("txtBoxDescricaoCadastroBoleto");
                    var categoryField = (FrameworkElement)window.FindName("comboBoxTipoCadastroBoleto");
                    var panel = (AdaptiveFormPanel)VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(description));
                    var a = description.TranslatePoint(new Point(), panel);
                    var b = categoryField.TranslatePoint(new Point(), panel);
                    Check(size.Width < 720 ? b.Y > a.Y : b.X > a.X, "Campos não reorganizados.");
                    Check(description.ActualWidth > 200, "Campo ficou estreito demais.");
                }
                if (tab == 1)
                {
                    var table = (DataGrid)window.FindName("dataGridBoletosFiltrados");
                    var search = (Button)window.FindName("btnBuscarBoletosFiltrados");
                    search.BringIntoView();
                    Layout(host, size);
                    search.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    Layout(host, size);
                    Check(table.Items.Count == 1, "Busca da interface falhou.");
                    Check(table.ActualHeight >= 70, "Filtros esconderam a tabela.");
                    var bounds = table.TransformToAncestor(host).TransformBounds(new Rect(table.RenderSize));
                    Check(bounds.Bottom <= size.Height, "Tabela ultrapassou a janela.");
                    Check(((ScrollViewer)window.FindName("searchFiltersScroll")).ViewportHeight > 0,
                        "Filtros sem área de rolagem.");
                }
                if (tab >= 2)
                {
                    var second = (FrameworkElement)window.FindName(tab == 2 ? "listBoxQuadroCategoria" : "dataGridTotalPorPeriodo");
                    Check(second.ActualHeight > 40, "Lista/tabela sem espaço.");
                    var section = tab == 2 ? (FrameworkElement)VisualTreeHelper.GetParent(second) : second;
                    Check(Grid.GetRow(section) == (size.Width < 840 ? 1 : 0), "Seções não reorganizadas.");
                }
                var bitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(host);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using var stream = File.Create(Path.Combine(output, $"{size.Width}x{size.Height}-aba{tab + 1}.png"));
                encoder.Save(stream);
            }
            Console.WriteLine($"OK: quatro abas em {size.Width} x {size.Height}");
        }
        boleto.Descricao = "Alterado";
        boletos.Save();
        Check(boletos.BuscarPorId(boleto.Id)!.Descricao == "Alterado", "Edição falhou.");
        boletos.Excluir(boleto);
        Check(!boletos.BuscarTodos().Any(), "Exclusão falhou.");
        Console.WriteLine("OK: criação, reabertura, consulta, edição e exclusão no SQLite.");
        app.Shutdown();
    }

    private static void Layout(FrameworkElement host, Size size)
    {
        for (int i = 0; i < 3; i++)
        {
            host.Measure(size);
            host.Arrange(new Rect(size));
            host.UpdateLayout();
            host.Dispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
        }
    }
}
