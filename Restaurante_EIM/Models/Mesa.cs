using System.Collections.Generic;

namespace Restaurante_EIM.Models
{
    public class Mesa
    {
        public int Capacidade { get; private set; }
        public int Id { get; private set; }
        public EstadoMesa Estado { get; set; }

        public List<Pedido> Pedidos { get; private set; }
        public List<Reserva> Reservas { get; private set; }

        public Mesa(int id, int capacidade)
        {
            this.Id = id;
            this.Capacidade = capacidade;
            this.Estado = EstadoMesa.Livre;
            this.Pedidos = new List<Pedido>();
            this.Reservas = new List<Reserva>();
        }

        public void AdicionarReserva(Reserva reserva) { }
        public void AdicionarPedido(Pedido pedido) { }
        public void LibertarMesa() { }
    }
}