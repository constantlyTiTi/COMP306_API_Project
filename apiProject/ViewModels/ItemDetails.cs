using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.ViewModels
{
    public class ItemDetails
    {
        public Item Item { get; set; }
        public IEnumerable<ItemFile> ItemFiles { get; set; }
    }
}
