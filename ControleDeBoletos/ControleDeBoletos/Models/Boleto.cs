using System;

namespace ControleDeBoletos.Models
{
    public class Boleto : BaseEntity
    {
        private long _emissaoTimestamp;
        private long _vencimentoTimestamp;

        public string Descricao { get; set; }
        public int TipoId { get; set; }
        public decimal Valor { get; set; }
        public DateTime Emissao
        {
            get
            {
                return new DateTime(_emissaoTimestamp);
            }
            set
            {
                _emissaoTimestamp = value.Ticks;
            }
        }
        public DateTime Vencimento
        {
            get
            {
                return new DateTime(_vencimentoTimestamp);
            }
            set
            {
                _vencimentoTimestamp = value.Ticks;
            }
        }
        public bool Situacao { get; set; }

        public TipoBoleto Tipo { get; set; }
    }
}
