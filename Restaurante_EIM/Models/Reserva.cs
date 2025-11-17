using Restaurante_EIM.Models;
using System;

namespace Restaurante_EIM.Models
{
    public class Reserva
    {
        private int id;
        private DateTime dataHora;
        private int numeroMesa;
        private string nomeCliente;
        private int numPessoas;
        private EstadoReserva estado;

        public int Id
        {
            get { return id; }
            internal set { id = value; }
        }

        public DateTime DataHora
        {
            get { return dataHora; }
            set { dataHora = value; }
        }

        public int NumeroMesa
        {
            get { return numeroMesa; }
            set { numeroMesa = value; }
        }

        public string NomeCliente
        {
            get { return nomeCliente; }
            set { nomeCliente = value; }
        }

        public int NumPessoas
        {
            get { return numPessoas; }
            set { numPessoas = value; }
        }

        public EstadoReserva Estado
        {
            get { return estado; }
            internal set { estado = value; }
        }

        public Reserva(int idInicial)
        {
            this.id = idInicial;
            this.dataHora = DateTime.Now;
            this.estado = EstadoReserva.Pendente;
        }

        public TimeSpan CalcularAntecedencia()
        {
            return DataHora.Subtract(DateTime.Now);
        }
    }
}