using Restaurante_EIM.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Restaurante_EIM.Services
{
    public class ReservaService
    {
        private List<Reserva> _reservas;
        private List<Mesa> _mesas;
        private int _proximoIdReserva = 1;

        public IReadOnlyList<Mesa> Mesas => _mesas;

        public ReservaService()
        {
            _reservas = new List<Reserva>();
            _mesas = new List<Mesa>();

            _mesas.Add(new Mesa(101, 4));
            _mesas.Add(new Mesa(102, 6));
            _mesas.Add(new Mesa(103, 2));
        }

        public bool AdicionarReserva(DateTime dataHora, int numPessoas, string nomeCliente)
        {
            TimeSpan antecedencia = dataHora.Subtract(DateTime.Now);
            if (antecedencia.TotalHours < 2) return false;

            Mesa mesaDisponivel = _mesas
                .Where(m => m.Capacidade >= numPessoas && m.Estado == EstadoMesa.Livre)
                .OrderBy(m => m.Capacidade)
                .FirstOrDefault();

            if (mesaDisponivel == null) return false;

            DateTime inicioConflito = dataHora.AddHours(-1);
            DateTime fimConflito = dataHora.AddHours(1);

            bool conflito = _reservas.Any(r =>
                r.NumeroMesa == mesaDisponivel.Id &&
                r.DataHora >= inicioConflito &&
                r.DataHora <= fimConflito &&
                r.Estado != EstadoReserva.Cancelada);

            if (conflito) return false;

            Reserva novaReserva = new Reserva(_proximoIdReserva++);
            novaReserva.DataHora = dataHora;
            novaReserva.NumeroMesa = mesaDisponivel.Id;
            novaReserva.NomeCliente = nomeCliente;
            novaReserva.NumPessoas = numPessoas;

            _reservas.Add(novaReserva);
            mesaDisponivel.Estado = EstadoMesa.Reservada;

            return true;
        }

        public List<Reserva> ListarReservas()
        {
            return _reservas;
        }

        public bool CancelarReserva(int reservaId)
        {
            Reserva reserva = _reservas.FirstOrDefault(r => r.Id == reservaId);

            if (reserva == null || reserva.Estado == EstadoReserva.Cancelada) return false;

            Mesa mesa = _mesas.FirstOrDefault(m => m.Id == reserva.NumeroMesa);
            if (mesa != null && mesa.Estado == EstadoMesa.Reservada)
            {
                mesa.Estado = EstadoMesa.Livre;
            }

            reserva.Estado = EstadoReserva.Cancelada;
            return true;
        }
    }
}