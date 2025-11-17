using Restaurante_EIM.Models;

namespace Restaurante_EIM.Models
{
    public class ItemPedido
    {
        private Item item;
        private int quantidade;
        private double precoUnitario;

        public Item Item
        {
            get { return item; }
            private set { item = value; }
        }

        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        public double PrecoUnitario
        {
            get { return precoUnitario; }
            private set { precoUnitario = value; }
        }

        public ItemPedido(Item item, int quantidade)
        {
            this.Item = item;
            this.Quantidade = quantidade;
            this.PrecoUnitario = item.Preco;
        }

        public double CalcularSubTotal()
        {
            return PrecoUnitario * Quantidade;
        }
    }
}