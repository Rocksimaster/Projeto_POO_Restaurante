namespace Restaurante_EIM.Users
{
    public class Gestor : Utilizador
    {
        public Gestor(int id, string nome, string username, string password)
            : base(id, nome, username, password)
        {
        }

        public override string GetPapel()
        {
            return "Gestor";
        }
    }
}