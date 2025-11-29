using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante_EIM.Models
{
    public class Ementa
    {
        private List<Item> items;

        public Ementa()
        {
            items = new List<Item>();
        }
        public void AdicionarItem(Item item)
        {
            items.Add(item);
        }

        public void RemoverItem(Item item) 
        { 
             items.Remove(item);
        }

        public List<Item> ConsultarEmenta()
        {
            return items;
        }
    }

    
}
