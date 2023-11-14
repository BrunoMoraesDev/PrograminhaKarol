using ControleDeBoletos.Data.Repositories;
using ControleDeBoletos.Enums;
using ControleDeBoletos.Models;
using ControleDeBoletos.ViewModels;
using ControleDeBoletos.ViewModels.TotalPorPeriodoViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ControleDeBoletos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoletoRepository _boletoRepository { get; set; }
        private TipoBoletoRepository _tipoBoletoRepository { get; set; }

        public ObservableCollection<Boleto> BoletosFiltrados { get; set; }

        public MainWindow(BoletoRepository boletoRepository, TipoBoletoRepository tipoBoletoRepository)
        {
            _boletoRepository = boletoRepository;
            _tipoBoletoRepository = tipoBoletoRepository;

            InitializeComponent();

            PreencherComboBoxCadastroBoletos();
            PreencherComboBoxFiltrosBuscaBoletos();
        }

        private void PreencherComboBoxFiltrosBuscaBoletos()
        {
            PreencherComboBoxsTipoBoleto();

            ItemComboBox<int?>[] diasDoMesArray = new ItemComboBox<int?>[]
            {
                new ItemComboBox<int?>(string.Empty, null), new ItemComboBox<int?>("1", 1), new ItemComboBox<int?>("2", 2), new ItemComboBox<int?>("3", 3), new ItemComboBox<int?>("4", 4), new ItemComboBox<int?>("5", 5), new ItemComboBox<int?>("6", 6), new ItemComboBox<int?>("7", 7), new ItemComboBox<int?>("8", 8), new ItemComboBox<int?>("9", 9), new ItemComboBox<int?>("10", 10), new ItemComboBox<int?>("11", 11), new ItemComboBox<int?>("12", 12), new ItemComboBox<int?>("13", 13), new ItemComboBox<int?>("14", 14), new ItemComboBox<int?>("15", 15), new ItemComboBox<int?>("16", 16), new ItemComboBox<int?>("17", 17), new ItemComboBox<int?>("18", 18), new ItemComboBox<int?>("19", 19), new ItemComboBox<int?>("20", 20), new ItemComboBox<int?>("21", 21), new ItemComboBox<int?>("22", 22), new ItemComboBox<int?>("23", 23), new ItemComboBox<int?>("24", 24), new ItemComboBox<int?>("25", 25), new ItemComboBox<int?>("26", 26), new ItemComboBox<int?>("27", 27), new ItemComboBox<int?>("28", 28), new ItemComboBox<int?>("29", 29), new ItemComboBox<int?>("30", 30), new ItemComboBox<int?>("31", 31)
            };

            ItemComboBox<int?>[] mesesDoAnoArray = new ItemComboBox<int?>[]
            {
                new ItemComboBox<int?>(string.Empty, null), new ItemComboBox<int?>("1", 1), new ItemComboBox<int?>("2", 2), new ItemComboBox<int?>("3", 3), new ItemComboBox<int?>("4", 4), new ItemComboBox<int?>("5", 5), new ItemComboBox<int?>("6", 6), new ItemComboBox<int?>("7", 7), new ItemComboBox<int?>("8", 8), new ItemComboBox<int?>("9", 9), new ItemComboBox<int?>("10", 10), new ItemComboBox<int?>("11", 11), new ItemComboBox<int?>("12", 12)
            };

            ItemComboBox<int?>[] anosArray = new ItemComboBox<int?>[]
            {
                new ItemComboBox<int?>(string.Empty, null), new ItemComboBox<int?>("2022", 2022), new ItemComboBox<int?>("2023", 2023), new ItemComboBox<int?>("2024", 2024), new ItemComboBox<int?>("2025", 2025), new ItemComboBox<int?>("2026", 2026), new ItemComboBox<int?>("2027", 2027), new ItemComboBox<int?>("2028", 2028), new ItemComboBox<int?>("2029", 2029), new ItemComboBox<int?>("2030", 2030), new ItemComboBox<int?>("2031", 2031), new ItemComboBox<int?>("2032", 2032), new ItemComboBox<int?>("2033", 2033)
            };

            ItemComboBox<TiposSituacao>[] situacoesArray = new ItemComboBox<TiposSituacao>[]
            {
                new ItemComboBox<TiposSituacao>("Todos", TiposSituacao.TODOS), new ItemComboBox<TiposSituacao>("Pagos", TiposSituacao.PAGOS), new ItemComboBox<TiposSituacao>("Pendentes", TiposSituacao.PENDENTES), new ItemComboBox<TiposSituacao>("Vencidos", TiposSituacao.VENCIDOS)
            };

            comboBoxFiltroDia.ItemsSource = diasDoMesArray;
            comboBoxFiltroMes.ItemsSource = mesesDoAnoArray;
            comboBoxFiltroAno.ItemsSource = anosArray;

            comboBoxFiltroDiaEmissao.ItemsSource = diasDoMesArray;
            comboBoxFiltroMesEmissao.ItemsSource = mesesDoAnoArray;
            comboBoxFiltroAnoEmissao.ItemsSource = anosArray;

            comboBoxFiltroSituacao.ItemsSource = situacoesArray;
        }

        private void PreencherComboBoxCadastroBoletos()
        {
            PreencherComboBoxsTipoBoleto();

            comboBoxParcelaCadastroBoleto.ItemsSource = new ItemComboBox<TiposPeriodoParcela>[]
            {
                new ItemComboBox<TiposPeriodoParcela>("Única", TiposPeriodoParcela.UNICA), new ItemComboBox<TiposPeriodoParcela>("Diária", TiposPeriodoParcela.DIARIA), new ItemComboBox<TiposPeriodoParcela>("Semanal", TiposPeriodoParcela.SEMANAL), new ItemComboBox<TiposPeriodoParcela>("Quinzenal", TiposPeriodoParcela.QUINZENAL), new ItemComboBox<TiposPeriodoParcela>("Mensal", TiposPeriodoParcela.MENSAL)
            };
        }

        private void btnCadastrarCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxNomeCadastroCategoria.Text == null || txtBoxNomeCadastroCategoria.Text == string.Empty)
            {
                MessageBox.Show("Não é possível criar categoria sem nome.");
                return;
            }

            TipoBoleto tipo = new()
            {
                Descricao = txtBoxNomeCadastroCategoria.Text,
                DataCadastro = DateTime.Now,
                DataAlteracao = null
            };

            _tipoBoletoRepository.Add(tipo);

            txtBoxNomeCadastroCategoria.Clear();
            PreencherComboBoxsTipoBoleto();
        }

        private void PreencherComboBoxsTipoBoleto()
        {
            IEnumerable<TipoBoleto> tiposBoleto = _tipoBoletoRepository.GetAll();
            comboBoxTipoCadastroBoleto.ItemsSource = tiposBoleto.ToList();

            List<ItemComboBox<TipoBoleto>> itensTipoBoleto = tiposBoleto.Select(tipoBoleto => new ItemComboBox<TipoBoleto>(tipoBoleto.Descricao, tipoBoleto)).ToList();
            itensTipoBoleto.Insert(0, new ItemComboBox<TipoBoleto>(string.Empty, null));
            comboBoxFiltroCategoria.ItemsSource = itensTipoBoleto.ToList();

            comboBoxFiltroCategoria.SelectedIndex = 0;
        }

        private void btnAtualizarQuadroCategoria_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<TipoBoleto> tiposBoleto = _tipoBoletoRepository.GetAll();

            listBoxQuadroCategoria.ItemsSource = tiposBoleto.ToList();
        }

        private void btnSalvarCadastroBoleto_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCamposCadastroBoleto()) return;

            TipoBoleto tipo = comboBoxTipoCadastroBoleto.SelectedItem as TipoBoleto;

            ItemComboBox<TiposPeriodoParcela> tipoPeriodoParcelaSelecionado = (ItemComboBox<TiposPeriodoParcela>)comboBoxParcelaCadastroBoleto.SelectionBoxItem;

            int numeroDeParcelas = string.IsNullOrEmpty(txtBoxNumeroParcelasCadastroBoleto.Text) ? 1 : int.Parse(txtBoxNumeroParcelasCadastroBoleto.Text);
            List<Boleto> boletos = new();
            switch (tipoPeriodoParcelaSelecionado.Valor)
            {
                case TiposPeriodoParcela.UNICA:

                    Boleto boleto = new()
                    {
                        Descricao = txtBoxDescricaoCadastroBoleto.Text,
                        TipoId = tipo.Id,
                        Valor = Convert.ToDecimal(txtBoxValorCadastroBoleto.Text),
                        Emissao = datePickEmissaoCadastroBoleto.SelectedDate.Value,
                        Vencimento = datePickVencimentoCadastroBoleto.SelectedDate.Value,
                        Situacao = checkBoxSituacaoCadastroBoleto.IsChecked.Value,
                    };
                    _boletoRepository.Add(boleto);

                    break;
                case TiposPeriodoParcela.DIARIA:
                    
                    for (int indice = 0; indice < numeroDeParcelas; indice++)
                    {
                        boletos.Add(new Boleto()
                        {
                            Descricao = $"{txtBoxDescricaoCadastroBoleto.Text} (Parcela {indice + 1}/{numeroDeParcelas})",
                            TipoId = tipo.Id,
                            Valor = Convert.ToDecimal(txtBoxValorCadastroBoleto.Text),
                            Emissao = datePickEmissaoCadastroBoleto.SelectedDate.Value,
                            Vencimento = datePickVencimentoCadastroBoleto.SelectedDate.Value.AddDays(indice),
                            Situacao = checkBoxSituacaoCadastroBoleto.IsChecked.Value,
                        });
                    }
                    _boletoRepository.Add(boletos);

                    break;
                case TiposPeriodoParcela.SEMANAL:

                    for (int indice = 0; indice < numeroDeParcelas; indice++)
                    {
                        boletos.Add(new Boleto()
                        {
                            Descricao = $"{txtBoxDescricaoCadastroBoleto.Text} (Parcela {indice + 1}/{numeroDeParcelas})",
                            TipoId = tipo.Id,
                            Valor = Convert.ToDecimal(txtBoxValorCadastroBoleto.Text),
                            Emissao = datePickEmissaoCadastroBoleto.SelectedDate.Value,
                            Vencimento = datePickVencimentoCadastroBoleto.SelectedDate.Value.AddDays(indice * 7),
                            Situacao = checkBoxSituacaoCadastroBoleto.IsChecked.Value,
                        });
                    }
                    _boletoRepository.Add(boletos);

                    break;
                case TiposPeriodoParcela.QUINZENAL:

                    for (int indice = 0; indice < numeroDeParcelas; indice++)
                    {
                        boletos.Add(new Boleto()
                        {
                            Descricao = $"{txtBoxDescricaoCadastroBoleto.Text} (Parcela {indice + 1}/{numeroDeParcelas})",
                            TipoId = tipo.Id,
                            Valor = Convert.ToDecimal(txtBoxValorCadastroBoleto.Text),
                            Emissao = datePickEmissaoCadastroBoleto.SelectedDate.Value,
                            Vencimento = datePickVencimentoCadastroBoleto.SelectedDate.Value.AddDays(indice * 15),
                            Situacao = checkBoxSituacaoCadastroBoleto.IsChecked.Value,
                        });
                    }
                    _boletoRepository.Add(boletos);

                    break;
                case TiposPeriodoParcela.MENSAL:

                    for (int indice = 0; indice < numeroDeParcelas; indice++)
                    {
                        boletos.Add(new Boleto()
                        {
                            Descricao = $"{txtBoxDescricaoCadastroBoleto.Text} (Parcela {indice + 1}/{numeroDeParcelas})",
                            TipoId = tipo.Id,
                            Valor = Convert.ToDecimal(txtBoxValorCadastroBoleto.Text),
                            Emissao = datePickEmissaoCadastroBoleto.SelectedDate.Value,
                            Vencimento = datePickVencimentoCadastroBoleto.SelectedDate.Value.AddMonths(indice),
                            Situacao = checkBoxSituacaoCadastroBoleto.IsChecked.Value,
                        });
                    }
                    _boletoRepository.Add(boletos);

                    break;
                default:
                    throw new NotImplementedException("Tipo de período de parcela não esperado pela aplicação");
            }

            LimparCamposCadastro();
        }

        private void LimparCamposCadastro()
        {
            txtBoxDescricaoCadastroBoleto.Clear();
            comboBoxTipoCadastroBoleto.SelectedItem = null;
            txtBoxValorCadastroBoleto.Clear();
            datePickEmissaoCadastroBoleto.SelectedDate = null;
            datePickVencimentoCadastroBoleto.SelectedDate = null;
            checkBoxSituacaoCadastroBoleto.IsChecked = false;
            comboBoxParcelaCadastroBoleto.SelectedItem = null;
            txtBoxNumeroParcelasCadastroBoleto.Clear();
        }

        private bool ValidarCamposCadastroBoleto()
        {
            if (string.IsNullOrEmpty(txtBoxDescricaoCadastroBoleto.Text))
            {
                MessageBox.Show("Preencha o campo de Descrição");
                return false;
            }

            if (comboBoxTipoCadastroBoleto.SelectionBoxItem == null)
            {
                MessageBox.Show("Selecione um elemento no campo de Tipo/Categoria");
                return false;
            }

            if (string.IsNullOrEmpty(txtBoxValorCadastroBoleto.Text))
            {
                MessageBox.Show("Preencha o campo de Valor");
                return false;
            }

            if (string.IsNullOrEmpty(datePickEmissaoCadastroBoleto.Text))
            {
                MessageBox.Show("Preencha o campo de Emissão");
                return false;
            }

            if (string.IsNullOrEmpty(datePickVencimentoCadastroBoleto.Text))
            {
                MessageBox.Show("Preencha o campo de Vencimento");
                return false;
            }

            ItemComboBox<TiposPeriodoParcela> tipoPeriodoParcelaSelecionado = (ItemComboBox<TiposPeriodoParcela>)comboBoxParcelaCadastroBoleto.SelectionBoxItem;
            if (tipoPeriodoParcelaSelecionado.Valor != TiposPeriodoParcela.UNICA)
            {
                if (string.IsNullOrEmpty(txtBoxNumeroParcelasCadastroBoleto.Text))
                {
                    MessageBox.Show("Preencha o campo com o número de parcelas OU selecione o boleto como parcela única");
                    return false;
                }
                
                if (!int.TryParse(txtBoxNumeroParcelasCadastroBoleto.Text, out _))
                {
                    MessageBox.Show("Preencha o campo de número de parcelas somente com números inteiros OU selecione o boleto como parcela única");
                    return false;
                }
            }

            return true;
        }

        private void txtBoxValorCadastroBoleto_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Usando uma expressão regular para verificar se o texto contém apenas dígitos
            // com ou sem decimais.
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[0-9]*(?:\,[0-9]*)?$"))
            {
                e.Handled = true; // Impede que o caractere não numérico seja inserido no TextBox.
            }
        }

        private void btnBuscarBoletosFiltrados_Click(object sender, RoutedEventArgs e)
        {
            string? descricao = txtBoxFiltroDescricao.Text;

            ItemComboBox<TiposSituacao> tipoSituacaoItemComboBox = (ItemComboBox<TiposSituacao>)comboBoxFiltroSituacao.SelectedItem;

            ItemComboBox<TipoBoleto> itemTipoBoleto = (ItemComboBox<TipoBoleto>)comboBoxFiltroCategoria.SelectedItem;

            ItemComboBox<int?> diaItemComboBoxVencimento = (ItemComboBox<int?>)comboBoxFiltroDia.SelectedItem;
            ItemComboBox<int?> mesItemComboBoxVencimento = (ItemComboBox<int?>)comboBoxFiltroMes.SelectedItem;
            ItemComboBox<int?> anoItemComboBoxVencimento = (ItemComboBox<int?>)comboBoxFiltroAno.SelectedItem;

            ItemComboBox<int?> diaItemComboBoxEmissao = (ItemComboBox<int?>)comboBoxFiltroDiaEmissao.SelectedItem;
            ItemComboBox<int?> mesItemComboBoxEmissao = (ItemComboBox<int?>)comboBoxFiltroMesEmissao.SelectedItem;
            ItemComboBox<int?> anoItemComboBoxEmissao = (ItemComboBox<int?>)comboBoxFiltroAnoEmissao.SelectedItem;

            List<Boleto> listaBoletos = _boletoRepository.GetFilteredBoletos(descricao, itemTipoBoleto.Valor, tipoSituacaoItemComboBox.Valor, diaItemComboBoxVencimento.Valor, mesItemComboBoxVencimento.Valor, anoItemComboBoxVencimento.Valor, diaItemComboBoxEmissao.Valor, mesItemComboBoxEmissao.Valor, anoItemComboBoxEmissao.Valor).ToList();

            BoletosFiltrados = new ObservableCollection<Boleto>(listaBoletos);

            dataGridBoletosFiltrados.ItemsSource = BoletosFiltrados;

            AtualizarSomasDeBoletosFiltrados();
        }

        private void btnSalvarAlteracoesBoletosFiltrados_Click(object sender, RoutedEventArgs e)
        {
            _boletoRepository.Save();
        }

        private async void dataGridBoletosFiltrados_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                await Task.Delay(100);

                //var columnName = e.Column.Header.ToString();
                //var newValue = (e.EditingElement as TextBox).Text;

                AtualizarSomasDeBoletosFiltrados();
            }
        }

        private void dataGridBoletosFiltrados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            decimal soma = 0;
            foreach (Boleto boleto in dataGridBoletosFiltrados.SelectedItems)
            {
                soma += boleto.Valor;
            }

            txtBlockSomaSelecionada.Text = soma == 0 ? "-----" : $"R$ {soma}";
        }

        private void AtualizarSomasDeBoletosFiltrados()
        {
            txtBlockSomaTotal.Text = $"R$ {BoletosFiltrados.Sum(boleto => boleto.Valor)}";
            txtBlockSomaAPagar.Text = $"R$ {BoletosFiltrados.Where(boleto => boleto.Situacao == false).Sum(boleto => boleto.Valor)}";
            txtBlockSomaVencida.Text = $"R$ {BoletosFiltrados.Where(boleto => boleto.Situacao == false && boleto.Vencimento < DateTime.Now).Sum(boleto => boleto.Valor)}";
        }

        private void btnApagarBoletosFiltradosSelecionados_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridBoletosFiltrados.SelectedItems.Count == 0) return;

            List<Boleto> boletosParaRemover = new();

            foreach (var linha in dataGridBoletosFiltrados.SelectedItems)
            {
                Boleto boletoDaLinha = (Boleto)linha;
                boletosParaRemover.Add(boletoDaLinha);
            }

            _boletoRepository.Delete(boletosParaRemover);

            foreach (var boleto in boletosParaRemover)
            {
                BoletosFiltrados.Remove(boleto);
            }
        }

        private void btnBuscarTotalPorPeriodo_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<TipoBoletoTotalPorPeriodoViewModel> tiposDeBoletosSelecionados = listBoxTotalPorPeriodo.Items
                .OfType<TipoBoletoTotalPorPeriodoViewModel>()
                .Where(item => item.EstaSelecionado)
                .ToList();

            DateTime? dataInicial = datePickDataInicialTotalPorPeriodo.SelectedDate;
            DateTime? dataFinal = datePickDataFinalTotalPorPeriodo.SelectedDate;

            IEnumerable<BoletosTotaisPorDiaTotalPorPeriodoViewModel> valoresDeBoletoTotaisPorPeriodo = _boletoRepository.BuscarBoletosTotaisPorPeriodo(tiposDeBoletosSelecionados, dataInicial, dataFinal).ToList();

            dataGridTotalPorPeriodo.ItemsSource = valoresDeBoletoTotaisPorPeriodo;
        }

        private void tabControlMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(e.Source is TabControl)) return;

            if (tabCadastroBoleto.IsSelected)
            {
                PreencherComboBoxCadastroBoletos();
            }
            else if (tabTotalPorPeriodo.IsSelected)
            {
                IEnumerable<TipoBoleto> todosTiposDeBoleto = _tipoBoletoRepository.GetAll();

                IEnumerable<TipoBoletoTotalPorPeriodoViewModel> todosTiposDeBoletoViewModel = todosTiposDeBoleto.Select(boleto =>
                new TipoBoletoTotalPorPeriodoViewModel()
                {
                    Id = boleto.Id,
                    Descricao = boleto.Descricao,
                    EstaSelecionado = true
                });

                listBoxTotalPorPeriodo.ItemsSource = todosTiposDeBoletoViewModel.ToList();
            }
        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                DatePicker? datePicker = sender as DatePicker;
                if (datePicker == null) return;

                string input = datePicker.Text;

                if (input.Length == 8)
                {
                    string dataFormatada = input.Insert(2, "/").Insert(5, "/");
                    datePicker.Text = dataFormatada;

                    DateTime dataConvertida;
                    if (DateTime.TryParseExact(dataFormatada, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dataConvertida))
                    {
                        datePicker.SelectedDate = dataConvertida;
                    }
                }
            }
        }
    }
}
