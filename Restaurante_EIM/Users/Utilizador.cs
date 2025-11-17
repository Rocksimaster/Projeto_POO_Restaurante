using System;

namespace Restaurante_EIM.Users
{
    public abstract class Utilizador
    {
        private int id;
        private string nome;
        private string username;
        protected string passwordHash;

        public int Id
        {
            get { return id; }
            private set { id = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public Utilizador(int id, string nome, string username, string password)
        {
            this.id = id;
            this.nome = nome;
            this.username = username;
            this.passwordHash = password;
        }

        public void SetPassword(string password)
        {
            this.passwordHash = password;
        }

        public bool VerificarPassword(string password)
        {
            return this.passwordHash == password;
        }

        public abstract string GetPapel();
    }
}