namespace Restaurante_EIM.Users
{
    public class EmpregadoMesa : Utilizador
    {
        public EmpregadoMesa(int id, string nome, string username, string password)
            : base(id, nome, username, password)
        {
        }

        public override string GetPapel()
        {
            return "Empregado de Mesa";
        }
    }
}