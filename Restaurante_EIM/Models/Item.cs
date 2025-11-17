namespace Restaurante_EIM.Models
{
    public class Item
    {
        private int id;
        private string nome;
        private double preco;

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

        public double Preco
        {
            get { return preco; }
            set { preco = value; }
        }

        public Item(int id, string nome, double preco)
        {
            this.id = id;
            this.nome = nome;
            this.preco = preco;
        }

        public void AtualizarPreço()
        {

        }
    }
}