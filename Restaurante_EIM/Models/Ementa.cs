using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurante_EIM.Models
{
    public class Ementa
    {
        private List<Item> _items;

        public Ementa()
        {
            _items = new List<Item>();
        }
        public void AdicionarItemEmenta(Item item)
        {
            _items.Add(item);
        }

        public bool RemoverItemEmenta(int itemId) 
        {
           Item item2 = _items.FirstOrDefault(i => i.Id == itemId);
            if (item2 != null)
            {
                _items.Remove(item2);
                return true;
            }
            return false;

        }

        public IReadOnlyList<Item> ConsultarEmenta()
        {
            return _items.AsReadOnly();
        }

        public Item ObterItemPorId(int itemId)
        {
            return _items.FirstOrDefault(i => i.Id == itemId);
        }
    }

    
}
