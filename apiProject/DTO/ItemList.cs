using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.DTO
{

    //this is the model to display item list page

    public class ItemList
    {
        [JsonPropertyName("items")]
        public IEnumerable<Item> Items { get; set; }
        [JsonPropertyName("paginate")]
        public Paginate Paginate { get; set; }
    }
}
