namespace Restaurante_EIM.Models
{
    public enum EstadoMesa
    {
        Livre,
        Reservada,
        Ocupada
    }

    public enum EstadoPedido
    {
        Aberto,
        Pronto,
        Servido,
        Pago
    }

    public enum EstadoReserva
    {
        Pendente,
        Confirmada,
        Cancelada
    }
}
