using Restaurante_EIM.Services;
using Restaurante_EIM.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Restaurante_EIM.Menus
{
    public static class BalcaoMenu
    {
        public static void MostrarMenu(ReservaService reservaService, PedidoService pedidoService)
        {
            bool aSair = false;

            while (!aSair)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("  MENU BALCÃO (Reservas e Pagamentos) ");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Gerir Reservas");
                Console.WriteLine("2. Processar Pagamento (Fechar Pedido)");
                Console.WriteLine("3. Voltar ao Login");
                Console.Write("Escolha uma opção: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        GerirReservasMenu(reservaService);
                        break;
                    case "2":
                        ProcessarPagamento(pedidoService);
                        break;
                    case "3":
                        aSair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Prima qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void GerirReservasMenu(ReservaService service)
        {
            bool aSair = false;
            while (!aSair)
            {
                Console.Clear();
                Console.WriteLine("--- GESTÃO DE RESERVAS ---");
                Console.WriteLine("1. Adicionar Nova Reserva");
                Console.WriteLine("2. Listar Reservas Ativas");
                Console.WriteLine("3. Voltar");
                Console.Write("Escolha uma opção: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AdicionarNovaReserva(service);
                        break;
                    case "2":
                        ListarReservas(service);
                        break;
                    case "3":
                        aSair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Prima qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void AdicionarNovaReserva(ReservaService service)
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR NOVA RESERVA ---");

            Console.Write("Nome do Cliente: ");
            string nomeCliente = Console.ReadLine();

            Console.Write("Contacto do cliente: ");
            string contacto = Console.ReadLine();

            Console.Write("Número de Pessoas: ");
            if (!int.TryParse(Console.ReadLine(), out int numPessoas) || numPessoas <= 0)
            {
                Console.WriteLine("Número de pessoas inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Data e Hora da Reserva (AAAA-MM-DD HH:MM): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dataHora))
            {
                Console.WriteLine("Formato de Data/Hora inválido.");
                Console.ReadKey();
                return;
            }

            if (service.AdicionarReserva(dataHora, numPessoas, nomeCliente, contacto))
            {
                Console.WriteLine("\nReserva adicionada com sucesso e mesa reservada!");
            }
            else
            {
                Console.WriteLine("\nERRO: Não foi possível adicionar a reserva.");
                Console.WriteLine("Motivos possíveis: Antecedência mínima de 2h, mesa indisponível/pequena, ou conflito de horário.");
            }
            Console.ReadKey();
        }

        private static void ListarReservas(ReservaService service)
        {
            Console.Clear();
            Console.WriteLine("--- RESERVAS ATIVAS (PENDENTES/CONFIRMADAS) ---");

            var reservas = service.ListarReservas()
                                  .Where(r => r.Estado != EstadoReserva.Cancelada)
                                  .OrderBy(r => r.DataHora);

            if (!reservas.Any())
            {
                Console.WriteLine("Nenhuma reserva ativa.");
            }
            else
            {
                foreach (var r in reservas)
                {
                    string tempo = r.CalcularAntecedencia().TotalHours > 0 ? "Futuro" : "Passado";
                    Console.WriteLine($"ID: {r.Id} | Nome Cliente: {r.NomeCliente,-20} | Contacto Cliente: {r.Contacto} | Pessoas: {r.NumPessoas} | Data: {r.DataHora:dd/MM HH:mm} | Estado: {r.Estado} ({tempo})");
                }
            }
            Console.ReadKey();
        }

        private static void ProcessarPagamento(PedidoService service)
        {
            Console.Clear();
            Console.WriteLine("--- PROCESSAR PAGAMENTO ---");

            var pedidosAbertos = service.ObterTodosPedidos()
                                        .Where(p => p.Estado == EstadoPedido.Servido || p.Estado == EstadoPedido.Pronto)
                                        .ToList();

            if (!pedidosAbertos.Any())
            {
                Console.WriteLine("Nenhum pedido pronto ou servido para ser pago.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Pedidos Ativos (Prontos/Servidos):");
            foreach (var p in pedidosAbertos)
            {
                Console.WriteLine($"ID: {p.Id} | Mesa: {p.NumeroMesa} | Total: {p.TotalPagar:C} | Estado: {p.Estado}");
            }

            Console.Write("\nIntroduza o ID do Pedido a Pagar: ");
            if (!int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Pedido pedido = service.ObterPedidoPorId(pedidoId);

            if (pedido == null)
            {
                Console.WriteLine($"Pedido {pedidoId} não encontrado.");
            }
            else if (pedido.Estado == EstadoPedido.Pago)
            {
                Console.WriteLine("Este pedido já foi pago.");
            }
            else
            {
                Console.WriteLine($"\nConfirma o pagamento do Pedido ID {pedido.Id} (Mesa {pedido.NumeroMesa}) no valor total de {pedido.TotalPagar:C}? (S/N)");
                if (Console.ReadLine().ToUpper() == "S")
                {
                    service.AtualizarEstado(pedido.Id, EstadoPedido.Pago);
                    Console.WriteLine("\nPagamento processado. Mesa libertada!");
                }
                else
                {
                    Console.WriteLine("Pagamento cancelado.");
                }
            }
            Console.ReadKey();
        }
    }
}