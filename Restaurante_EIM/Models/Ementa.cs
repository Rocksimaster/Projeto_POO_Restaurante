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
        public void AdicionarItemEmenta(Item item)
        {
            items.Add(item);
        }

        public bool RemoverItemEmenta(int itemId) 
        {
           Item item2 = items.FirstOrDefault(i => i.Id == itemId);
            if (item2 != null)
            {
                items.Remove(item2);
                return true;
            }
            return false;

        }

        public List<Item> ConsultarEmenta()
        {
            return items;
        }
    }

    
}
