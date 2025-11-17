using Restaurante_EIM.Models;
using System.Collections.Generic;
using System.Linq;

namespace Restaurante_EIM.Models
{
    public class Pedido
    {
        private int id;
        private List<ItemPedido> items;
        private EstadoPedido estado;
        private double totalPagar;
        private int numeroMesa;

        public int Id { get; private set; }
        public List<ItemPedido> Items { get; private set; }
        public EstadoPedido Estado { get; set; }
        public double TotalPagar { get; private set; }
        public int NumeroMesa { get; internal set; }


        public Pedido(int id)
        {
            this.Id = id;
            this.Items = new List<ItemPedido>();
            this.Estado = EstadoPedido.Aberto;
            this.TotalPagar = 0;
            this.NumeroMesa = 0;
        }

        public void AdicionarItem(Item item, int quantidade)
        {
            Items.Add(new ItemPedido(item, quantidade));
            CalcularTotalPedido();
        }

        public bool RemoverLinha(int itemId)
        {
            ItemPedido linhaParaRemover = Items.FirstOrDefault(lp => lp.Item.Id == itemId);

            if (linhaParaRemover != null)
            {
                Items.Remove(linhaParaRemover);
                CalcularTotalPedido();
                return true;
            }
            return false;
        }

        public void AtualizarEstado(EstadoPedido estado)
        {
            this.Estado = estado;
        }

        public void CalcularTotalPedido()
        {
            double total = 0;
            foreach (var linha in Items)
            {
                total += linha.CalcularSubTotal();
            }
            TotalPagar = total;
        }
    }
}