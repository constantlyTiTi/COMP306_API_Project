using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.ViewModels
{
    //this is the model to display item list page
    public class ItemList
    {
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<string> Uploaders { get; set; } 
    }
}
