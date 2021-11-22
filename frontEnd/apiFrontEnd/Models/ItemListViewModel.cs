using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    class ItemListViewModel
    {
        [JsonPropertyName("items")]
        public IEnumerable<Item> Items { get; set; }
        [JsonPropertyName("paginate")]
        public Paginate Paginate { get; set; }
    }
}
