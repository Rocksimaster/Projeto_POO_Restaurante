using Restaurante_EIM.Services;
using Restaurante_EIM.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Restaurante_EIM.Menus
{
    public static class EmpregadoMenu
    {
        public static void MostrarMenu(PedidoService pedidoService, ReservaService reservaService, Ementa ementa)
        {
            bool aSair = false;

            while (!aSair)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("     MENU EMPREGADO DE MESA (Pedidos)   ");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Abrir Novo Pedido para Mesa");
                Console.WriteLine("2. Visualizar Ementa");
                Console.WriteLine("3. Adicionar Itens a Pedido Existente");
                Console.WriteLine("4. Remover Item de Pedido Aberto");
                Console.WriteLine("5. Marcar Pedido como PRONTO");
                Console.WriteLine("6. Entregar Pedido (Atualizar para Servido)");
                Console.WriteLine("7. Voltar ao Login");
                Console.Write("Escolha uma opção: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AbrirNovoPedido(pedidoService, reservaService);
                        break;
                    case "2":
                        VisualizarEmenta(ementa);
                        break;
                    case "3":
                        AdicionarItens(pedidoService, ementa);
                        break;
                    case "4":
                        RemoverItens(pedidoService);
                        break;
                    case "5":
                        MarcarPedidoPronto(pedidoService);
                        break;
                    case "6":
                        EntregarPedido(pedidoService);
                        break;
                    case "7":
                        aSair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Prima qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void AbrirNovoPedido(PedidoService pedidoService, ReservaService reservaService)
        {
            Console.Clear();
            Console.WriteLine("--- ABRIR NOVO PEDIDO ---");

            var mesasDisponiveis = reservaService.Mesas
                .Where(m => m.Estado == EstadoMesa.Livre || m.Estado == EstadoMesa.Reservada)
                .ToList();

            if (!mesasDisponiveis.Any())
            {
                Console.WriteLine("Nenhuma mesa livre ou reservada para iniciar um pedido.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nMesas Disponíveis:");
            foreach (var m in mesasDisponiveis)
            {
                Console.WriteLine($"Mesa: {m.Id} | Capacidade: {m.Capacidade} | Estado: {m.Estado}");
            }

            Console.Write("\nIntroduza o NÚMERO DA MESA para abrir o pedido: ");
            if (!int.TryParse(Console.ReadLine(), out int numMesa))
            {
                Console.WriteLine("Número de mesa inválido.");
                Console.ReadKey();
                return;
            }

            Mesa mesaSelecionada = mesasDisponiveis.FirstOrDefault(m => m.Id == numMesa);

            if (mesaSelecionada != null)
            {
                Pedido novoPedido = pedidoService.CriarNovoPedido(numMesa);
                Console.WriteLine($"\nPedido ID {novoPedido.Id} aberto para a Mesa {numMesa}. Estado alterado para Ocupada.");
            }
            else
            {
                Console.WriteLine("\n❌ Mesa não encontrada ou não está Livre/Reservada.");
            }
            Console.ReadKey();
        }

        private static void VisualizarEmenta(Ementa ementa)
        {
            Console.Clear();
            Console.WriteLine("--- EMENTA DO RESTAURANTE ---");

            var items = ementa.ConsultarEmenta();

            if (!items.Any())
            {
                Console.WriteLine("A ementa está vazia.");
            }
            else
            {
                Console.WriteLine("ID | Nome                       | Preço");
                Console.WriteLine("---|----------------------------|------");
                foreach (var item in items)
                {
                    Console.WriteLine($"{item.Id,-2} | {item.Nome,-26} | {item.Preco:C}");
                }
            }
            Console.WriteLine("\nPrima qualquer tecla para voltar ao menu.");
            Console.ReadKey();
        }

        private static void AdicionarItens(PedidoService pedidoService, Ementa ementa)
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR ITENS AO PEDIDO ---");

            Console.Write("Introduza o ID do Pedido a modificar: ");
            if (!int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                Console.WriteLine("ID de pedido inválido.");
                Console.ReadKey();
                return;
            }

            Pedido pedido = pedidoService.ObterPedidoPorId(pedidoId);

            if (pedido == null || pedido.Estado != EstadoPedido.Aberto)
            {
                Console.WriteLine("\n❌ Pedido não encontrado ou não está no estado 'Aberto' para ser modificado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n--- Ementa do Restaurante (Pedido ID: {pedido.Id}) ---");
            VisualizarEmenta(ementa);

            bool adicionarMais = true;
            while (adicionarMais)
            {
                Console.Write("Introduza o ID do Item (ou 'FIM' para terminar): ");
                string inputItem = Console.ReadLine();
                if (inputItem.ToUpper() == "FIM")
                {
                    adicionarMais = false;
                    break;
                }

                if (!int.TryParse(inputItem, out int itemId))
                {
                    Console.WriteLine("ID de Item inválido.");
                    continue;
                }

                Console.Write("Quantidade: ");
                if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
                {
                    Console.WriteLine("Quantidade inválida.");
                    continue;
                }

                if (pedidoService.AdicionarItemAoPedido(pedidoId, itemId, quantidade))
                {
                    Console.WriteLine($"\n✅ Item ID {itemId} adicionado. Novo Total: {pedido.TotalPagar:C}");
                }
                else
                {
                    Console.WriteLine("\n❌ ERRO: Item não encontrado ou pedido não pode ser modificado.");
                }
            }
            Console.ReadKey();
        }

        private static void RemoverItens(PedidoService service)
        {
            Console.Clear();
            Console.WriteLine("--- REMOVER ITEM DE PEDIDO ABERTO ---");

            Console.Write("Introduza o ID do Pedido a modificar: ");
            if (!int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                Console.WriteLine("ID de pedido inválido.");
                Console.ReadKey();
                return;
            }

            Pedido pedido = service.ObterPedidoPorId(pedidoId);

            if (pedido == null || pedido.Estado != EstadoPedido.Aberto)
            {
                Console.WriteLine("\n❌ Pedido não encontrado ou não está no estado 'Aberto' para ser modificado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nItens atuais no Pedido ID {pedido.Id} (Total: {pedido.TotalPagar:C}):");
            foreach (var linha in pedido.Items)
            {
                Console.WriteLine($"  Item ID: {linha.Item.Id} | {linha.Item.Nome} x{linha.Quantidade}");
            }

            Console.Write("\nIntroduza o ID do Item a REMOVER: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("ID de Item inválido.");
                Console.ReadKey();
                return;
            }

            if (service.RemoverItemDoPedido(pedidoId, itemId))
            {
                Console.WriteLine($"\n✅ Item ID {itemId} removido com sucesso. Novo Total: {pedido.TotalPagar:C}");
            }
            else
            {
                Console.WriteLine("\n❌ Erro: Item não encontrado neste pedido.");
            }
            Console.ReadKey();
        }

        private static void MarcarPedidoPronto(PedidoService service)
        {
            Console.Clear();
            Console.WriteLine("--- MARCAR PEDIDO COMO PRONTO (Cozinha/Preparo) ---");

            var pedidosEmAberto = service.ListarPedidosPorEstado(EstadoPedido.Aberto);

            if (!pedidosEmAberto.Any())
            {
                Console.WriteLine("Nenhum pedido em aberto para ser preparado.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Pedidos em Aberto:");
            foreach (var p in pedidosEmAberto)
            {
                Console.WriteLine($"ID: {p.Id} | Mesa: {p.NumeroMesa} | Total: {p.TotalPagar:C}");
            }

            Console.Write("\nIntroduza o ID do Pedido que está PRONTO: ");
            if (!int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Pedido pedido = service.ObterPedidoPorId(pedidoId);

            if (pedido == null || pedido.Estado != EstadoPedido.Aberto)
            {
                Console.WriteLine("❌ Erro: Pedido não encontrado ou já foi marcado como pronto/servido/pago.");
            }
            else
            {
                service.AtualizarEstado(pedido.Id, EstadoPedido.Pronto);
                Console.WriteLine($"\n✅ Pedido ID {pedido.Id} (Mesa {pedido.NumeroMesa}) marcado como PRONTO.");
                Console.WriteLine("O empregado de mesa já pode fazer a entrega (opção 6).");
            }
            Console.ReadKey();
        }

        private static void EntregarPedido(PedidoService pedidoService)
        {
            Console.Clear();
            Console.WriteLine("--- ATUALIZAR PEDIDO PARA SERVIDO ---");

            var pedidosProntos = pedidoService.ListarPedidosPorEstado(EstadoPedido.Pronto);

            if (!pedidosProntos.Any())
            {
                Console.WriteLine("Nenhum pedido está pronto para ser entregue (estado 'Pronto').");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Pedidos Prontos para Entrega:");
            foreach (var p in pedidosProntos)
            {
                Console.WriteLine($"ID: {p.Id} | Mesa: {p.NumeroMesa} | Total: {p.TotalPagar:C}");
            }

            Console.Write("\nIntroduza o ID do Pedido ENTREGUE: ");
            if (!int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                Console.WriteLine("ID inválido.");
                Console.ReadKey();
                return;
            }

            Pedido pedido = pedidoService.ObterPedidoPorId(pedidoId);

            if (pedido == null || pedido.Estado != EstadoPedido.Pronto)
            {
                Console.WriteLine("❌ Erro: Pedido não encontrado ou não está pronto para ser servido.");
            }
            else
            {
                pedidoService.AtualizarEstado(pedido.Id, EstadoPedido.Servido);
                Console.WriteLine($"\n✅ Pedido ID {pedido.Id} entregue na Mesa {pedido.NumeroMesa}. Estado atualizado para 'Servido'.");
            }
            Console.ReadKey();
        }
    }
}