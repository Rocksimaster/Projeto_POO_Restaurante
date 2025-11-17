using Restaurante_EIM.Services;
using Restaurante_EIM.Users;
using Restaurante_EIM.Menus;
using System;

namespace Restaurante_EIM
{
    internal class Program
    {
        private static UserService _userService = new UserService();
        private static ReservaService _reservaService = new ReservaService();
        private static PedidoService _pedidoService = new PedidoService();

        static void Main(string[] args)
        {
            bool aSair = false;

            while (!aSair)
            {
                Console.Clear();
                Console.WriteLine("======================================");
                Console.WriteLine("    IPCASOLUTIONS | RESTAURANTE  ");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Autenticar");
                Console.WriteLine("2. Sair");
                Console.Write("Escolha uma opção: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ProcessarLogin();
                        break;
                    case "2":
                        aSair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Prima qualquer tecla para continuar.");
                        Console.ReadKey();
                        break;
                }
            }

            Console.WriteLine("Sistema encerrado. Obrigado e adeus!");
        }

        private static void ProcessarLogin()
        {
            Console.Clear();
            Console.WriteLine("--- AUTENTICAÇÃO ---");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            Utilizador utilizador = _userService.Autenticar(username, password);

            if (utilizador != null)
            {
                Console.WriteLine($"Autenticação bem-sucedida! Bem-vindo(a), {utilizador.Nome} ({utilizador.GetPapel()}).");
                ChamarMenuUtilizador(utilizador);
            }
            else
            {
                Console.WriteLine("Erro: Credenciais inválidas. Prima qualquer tecla para continuar.");
                Console.ReadKey();
            }
        }

        private static void ChamarMenuUtilizador(Utilizador u)
        {
            switch (u.GetPapel())
            {
                case "Gestor":
                    GestorMenu.MostrarMenu(_userService, _reservaService, _pedidoService, u as Gestor);
                    break;
                case "Empregado de Mesa":
                    EmpregadoMenu.MostrarMenu(_pedidoService, _reservaService);
                    break;
                case "Empregado de Balcão":
                    BalcaoMenu.MostrarMenu(_reservaService, _pedidoService);
                    break;
                default:
                    Console.WriteLine("Erro de configuração de funçãoa. Prima qualquer tecla para continuar.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}