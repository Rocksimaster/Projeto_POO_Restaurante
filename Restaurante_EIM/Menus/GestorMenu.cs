using Restaurante_EIM.Models;
using Restaurante_EIM.Services;
using Restaurante_EIM.Users;
using System;
using System.Collections.Generic;

namespace Restaurante_EIM.Menus
{
    public static class GestorMenu
    {

        public static void MostrarMenu(
            UserService userService,
            ReservaService reservaService,
            PedidoService pedidoService,
            Gestor gestor, 
            Ementa ementa)
        {
            bool aSair = false;

            while (!aSair)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine($"  MENU ADMINISTRATIVO | Gestor: {gestor.Nome}  ");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Gerir Funcionários (Adicionar/Listar)");
                Console.WriteLine("2. Cancelar Reserva ");
                Console.WriteLine("3. Cancelar Pedido ");
                Console.WriteLine("4. Adicionar Item Ementa ");
                Console.WriteLine("5. Remover Item Ementa ");
                Console.WriteLine("6. Voltar ao Login ");
                Console.Write("Escolha uma opção: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        GerirFuncionarios(userService);
                        break;
                    case "2":
                        CancelarReserva(reservaService);
                        break;
                    case "3":
                        CancelarPedido(pedidoService);
                        break;
                    case "4":
                        AdicionarItemEmenta(ementa);
                        break;
                    case "5":
                        RemoverItemEmenta(ementa);
                        break;
                    case "6":
                        aSair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Prima qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        private static void GerirFuncionarios(UserService service)
        {
            Console.Clear();
            Console.WriteLine("--- GESTÃO DE FUNCIONÁRIOS ---");
            Console.WriteLine("1. Adicionar Novo Funcionário");
            Console.WriteLine("2. Listar Funcionários");
            Console.Write("Opção: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AdicionarFuncionario(service);
                    break;
                case "2":
                    ListarFuncionarios(service);
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    Console.ReadKey();
                    break;
            }
        }

        private static void AdicionarFuncionario(UserService service)
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR NOVO FUNCIONÁRIO ---");

            Console.Write("Nome Completo: ");
            string nome = Console.ReadLine();

            Console.Write("Username (login): ");
            string username = Console.ReadLine();

            Console.Write("Password inicial: ");
            string password = Console.ReadLine();

            Console.WriteLine("\nTipo de Funcionário:");
            Console.WriteLine("1. Empregado de Mesa");
            Console.WriteLine("2. Empregado de Balcão");
            Console.Write("Escolha (1 ou 2): ");
            string tipo = Console.ReadLine();

            Utilizador novoFuncionario = null;
            int novoId = service.ObterNovoId();

            if (tipo == "1")
            {
                novoFuncionario = new EmpregadoMesa(novoId, nome, username, password);
            }
            else if (tipo == "2")
            {
                novoFuncionario = new EmpregadoBalcao(novoId, nome, username, password);
            }

            if (novoFuncionario != null && service.AdicionarUtilizador(novoFuncionario))
            {
                Console.WriteLine($"\n✅ Sucesso! {novoFuncionario.Nome} (Papel: {novoFuncionario.GetPapel()}) adicionado.");
            }
            else
            {
                Console.WriteLine("\n❌ Erro: Tipo de funcionário inválido ou Username já existe.");
            }
            Console.ReadKey();
        }

        private static void ListarFuncionarios(UserService service)
        {
            Console.Clear();
            Console.WriteLine("--- LISTA DE FUNCIONÁRIOS ---");

            List<Utilizador> lista = service.ListarTodos(); 

            if (lista == null || lista.Count == 0)
            {
                Console.WriteLine("Nenhum funcionário registado.");
            }
            else
            {
                foreach (var u in lista)
                {
                    Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome,-20} | Username: {u.Username,-10} | Papel: {u.GetPapel()}");
                }
            }
            Console.ReadKey();
        }

        private static void CancelarReserva(ReservaService service)
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR RESERVA ---");
            Console.WriteLine("⚠️ Apenas para o Gestor. Esta ação não pode ser desfeita.");
            Console.Write("Introduza o ID da Reserva a cancelar: ");

            if (int.TryParse(Console.ReadLine(), out int reservaId))
            {
                if (service.CancelarReserva(reservaId))
                {
                    Console.WriteLine("\n✅ Reserva cancelada com sucesso e mesa libertada.");
                }
                else
                {
                    Console.WriteLine("\n❌ Erro: Reserva não encontrada ou já estava cancelada.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
        }
        private static void AdicionarItemEmenta(Ementa ementa)
        {
            Console.Clear();
            Console.WriteLine("--- ADICIONAR ITEM ---");
            Console.WriteLine(" Introduza o nome do item: ");
            string nome = Console.ReadLine();
            Console.WriteLine(" Introduza o preço do item: ");
            double preco = Convert.ToDouble(Console.ReadLine());
            Item item1 = new Item(nome, preco);
            ementa.AdicionarItemEmenta(item1);
            Console.WriteLine("\n✅ Item adicionado à ementa com sucesso.");
            Console.ReadKey();
        }

        private static void RemoverItemEmenta(Ementa ementa)
        {
            Console.Clear();
            Console.WriteLine("--- Remover Item ---");
            Console.WriteLine("Introduza o id do item: ");
            int id = Convert.ToInt32(Console.ReadLine());
            bool sucesso = ementa.RemoverItemEmenta(id);
            if (sucesso == true)
            {
                Console.WriteLine("\n✅ Item removido da ementa com sucesso.");
            }
            else
            {
                Console.WriteLine("\n❌ Erro: Item não encontrado.");
            }
            Console.ReadKey();

        }

        private static void ConsultarEmenta(Ementa ementa)
        {
            Console.Clear();

            Console.WriteLine($"ID: {u.Id} | Nome: {u.Nome,-20} | Username: {u.Username,-10} | Papel: {u.GetPapel()}");
        }
        private static void CancelarPedido(PedidoService service)
        {
            Console.Clear();
            Console.WriteLine("--- CANCELAR PEDIDO ---");
            Console.WriteLine("⚠️ Apenas para o Gestor. Esta ação remove o pedido do sistema.");
            Console.Write("Introduza o ID do Pedido a cancelar: ");

            if (int.TryParse(Console.ReadLine(), out int pedidoId))
            {
                if (service.CancelarPedido(pedidoId))
                {
                    Console.WriteLine("\n✅ Pedido cancelado e removido do sistema.");
                }
                else
                {
                    Console.WriteLine("\n❌ Erro: Pedido não encontrado ou já estava pago.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
        }
    }
}
