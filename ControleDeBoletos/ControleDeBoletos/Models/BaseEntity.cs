using System;
using System.Windows.Markup;

namespace ControleDeBoletos.Models
{
    public class BaseEntity
    {
        private long _dataCadastroTimestamp;
        private long? _dataAlteracaoTimestamp;

        public int Id { get; set; }
        public DateTime DataCadastro 
        {
            get
            {
                return new DateTime(_dataCadastroTimestamp);
            }
            set 
            {
                _dataCadastroTimestamp = value.Ticks;
            } 
        }
        public DateTime? DataAlteracao
        {
            get
            {
                if (_dataAlteracaoTimestamp == null) return null;

                return new DateTime(_dataAlteracaoTimestamp.Value);
            }
            set
            {
                if (value == null) _dataAlteracaoTimestamp = null;

                _dataAlteracaoTimestamp = value?.Ticks;
            }
        }
    }
}
