namespace ControleDeBoletos.Models
{
    public class ItemComboBox<T>
    {
        public ItemComboBox(string descricao, T? valor)
        {
            Descricao = descricao;
            Valor = valor;
        }
        public string Descricao { get; set; }
        public T? Valor { get; set; }
    }
}
