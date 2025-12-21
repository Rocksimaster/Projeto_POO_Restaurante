using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante_EIM.Models
{
    public class RelatorioMensal
    {
        public List<Reserva> reservas;
        public List<Pedido> pedidos;
        public RelatorioMensal(List<Reserva> reservas, List<Pedido> pedidos)
        {
            this.reservas = reservas;
            this.pedidos = pedidos;
        }

        public List<Reserva> ReservasMensais()
        {
            List<Reserva> reservasMensais = new List<Reserva>();
            foreach(Reserva reserva in reservas)
            {
                int mesAtual = DateTime.Now.Month;
                int mesReserva = reserva.DataHora.Month;
                if(mesReserva == mesAtual)
                {
                    reservasMensais.Add(reserva);
                }
            }
            return reservasMensais;

        }
        
        public List<Pedido> PedidosMensais()
        {
            List<Pedido> pedidosMensais = new List<Pedido>();
            foreach(Pedido pedido in pedidos)
            {
                int mesatual = DateTime.Now.Month;
                int mespedido = pedido.DataHora.Month;
                if(mespedido == mesatual)
                {
                    pedidosMensais.Add(pedido);
                }
            }
            return pedidosMensais;
        }
    }
}
