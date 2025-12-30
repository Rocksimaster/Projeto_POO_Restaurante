using Restaurante_EIM.Users;
using System.Collections.Generic;
using System.Linq;

namespace Restaurante_EIM.Services
{
    public class UserService
    {
        private List<Utilizador> _utilizadores;
        private int _proximoId = 1;

        public UserService()
        {
            _utilizadores = new List<Utilizador>();
            _utilizadores.Add(new Gestor(_proximoId++, "Gestor Admin", "admin", "123"));
            _utilizadores.Add(new EmpregadoBalcao(_proximoId, "Maria", "maria", "123"));
            _utilizadores.Add(new EmpregadoMesa(_proximoId, "Simao", "simao", "123"));
        }

        public Utilizador Autenticar(string username, string password)
        {
            Utilizador user = _utilizadores.FirstOrDefault(u => u.Username == username);

            if (user != null && user.VerificarPassword(password))
            {
                return user;
            }
            return null;
        }

        public int ObterNovoId()
        {
            return _proximoId++;
        }

        public List<Utilizador> ListarTodos()
        {
            return _utilizadores;
        }

        public bool AdicionarUtilizador(Utilizador novoUtilizador)
        {
            if (_utilizadores.Any(u => u.Username == novoUtilizador.Username))
            {
                return false;
            }

            _utilizadores.Add(novoUtilizador);
            return true;
        }
    }
}