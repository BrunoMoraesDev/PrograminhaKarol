using ControleDeBoletos.Models;
using System;
using System.ComponentModel;

namespace ControleDeBoletos.ViewModels
{
    public class BoletoFiltradoViewModel : BaseEntity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Boleto Boleto { get; set; }

        private string _descricao = string.Empty;
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    Boleto.Descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                }
            }
        }

        private int _tipoId;
        public int TipoId
        {
            get => _tipoId;
            set
            {
                if (_tipoId != value)
                {
                    _tipoId = value;
                    Boleto.TipoId = value;
                    OnPropertyChanged(nameof(TipoId));
                }
            }
        }

        private decimal _valor;
        public decimal Valor
        {
            get => _valor;
            set
            {
                if (_valor != value)
                {
                    _valor = value;
                    Boleto.Valor = value;
                    OnPropertyChanged(nameof(Valor));
                }
            }
        }

        private DateTime _emissao;
        public DateTime Emissao
        {
            get => _emissao;
            set
            {
                if (_emissao != value)
                {
                    _emissao = value;
                    Boleto.Emissao = value;
                    OnPropertyChanged(nameof(Emissao));
                }
            }
        }

        private DateTime _vencimento;
        public DateTime Vencimento
        {
            get => _vencimento;
            set
            {
                if (_vencimento != value)
                {
                    _vencimento = value;
                    Boleto.Vencimento = value;
                    OnPropertyChanged(nameof(Vencimento));
                    DiasVencido = (DateTime.Today > _vencimento.Date)
                        ? $"{(DateTime.Today - _vencimento.Date).Days}"
                        : string.Empty;
                }
            }
        }

        private bool _situacao;
        public bool Situacao
        {
            get => _situacao;
            set
            {
                if (_situacao != value)
                {
                    _situacao = value;
                    Boleto.Situacao = value;
                    OnPropertyChanged(nameof(Situacao));
                }
            }
        }

        private string _diasVencido = string.Empty;
        public string DiasVencido
        {
            get => _diasVencido;
            private set
            {
                if (_diasVencido != value)
                {
                    _diasVencido = value;
                    OnPropertyChanged(nameof(DiasVencido));
                }
            }
        }

        private TipoBoleto _tipo;
        public TipoBoleto Tipo
        {
            get => _tipo;
            set
            {
                if (_tipo != value)
                {
                    _tipo = value;
                    Boleto.Tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
