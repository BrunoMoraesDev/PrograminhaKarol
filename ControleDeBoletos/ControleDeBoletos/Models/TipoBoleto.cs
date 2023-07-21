using System.Windows.Controls;
using System.Xml.Linq;

namespace ControleDeBoletos.Models
{
    public class TipoBoleto : BaseEntity
    {
        public string Descricao { get; set; }

        public override string ToString()
        {
            return Descricao;
        }
    }
}
