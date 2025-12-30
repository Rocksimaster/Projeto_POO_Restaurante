using Restaurante_EIM.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Restaurante_EIM.Services
{
    public class PedidoService
    {
        private List<Pedido> _pedidos;
        private Ementa _ementa;
        private int _proximoIdPedido = 1;
        private ReservaService _reservaService;

        public PedidoService(Ementa ementa, ReservaService reservaService)
        {
            _pedidos = new List<Pedido>();
            _reservaService = reservaService;
            _ementa = ementa;
        }


        public bool RemoverItemDoPedido(int pedidoId, int itemId)
        {
            Pedido pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);

            if (pedido == null || pedido.Estado != EstadoPedido.Aberto)
            {
                return false;
            }

            return pedido.RemoverLinha(itemId);
        }

        public Pedido CriarNovoPedido(int numeroMesa)
        {
            Pedido novoPedido = new Pedido(_proximoIdPedido++);
            novoPedido.NumeroMesa = numeroMesa;
            _pedidos.Add(novoPedido);

            if (_reservaService != null)
            {
                Mesa mesa = _reservaService.Mesas.FirstOrDefault(m => m.Id == numeroMesa);
                if (mesa != null)
                {
                    mesa.Estado = EstadoMesa.Ocupada;
                }
            }

            return novoPedido;
        }

        public bool AdicionarItemAoPedido(int pedidoId, int itemId, int quantidade)
        {
            Pedido pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);
            Item item = _ementa.ObterItemPorId(itemId);

            if (pedido != null && item != null && pedido.Estado == EstadoPedido.Aberto)
            {
                pedido.AdicionarItem(item, quantidade);
                return true;
            }
            return false;
        }

        public bool AtualizarEstado(int pedidoId, EstadoPedido novoEstado)
        {
            Pedido pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);

            if (pedido == null) return false;

            pedido.Estado = novoEstado;

            // Lógica de liberação da mesa após o pagamento (tarefa do Balcão)
            if (novoEstado == EstadoPedido.Pago && _reservaService != null)
            {
                Mesa mesa = _reservaService.Mesas.FirstOrDefault(m => m.Id == pedido.NumeroMesa);
                if (mesa != null)
                {
                    mesa.Estado = EstadoMesa.Livre;
                }
            }

            return true;
        }

        public bool CancelarPedido(int pedidoId)
        {
            Pedido pedido = _pedidos.FirstOrDefault(p => p.Id == pedidoId);

            if (pedido == null || pedido.Estado == EstadoPedido.Pago)
            {
                return false;
            }

            _pedidos.Remove(pedido);

            return true;
        }

        public Pedido ObterPedidoPorId(int pedidoId)
        {
            return _pedidos.FirstOrDefault(p => p.Id == pedidoId);
        }

        public List<Pedido> ListarPedidosPorEstado(EstadoPedido estado)
        {
            return _pedidos.Where(p => p.Estado == estado).ToList();
        }

        public List<Pedido> ObterTodosPedidos()
        {
            return _pedidos;
        }
    }
}