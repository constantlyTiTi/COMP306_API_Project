using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    class OrderListViewModel
    {
        [JsonPropertyName("orders")]
        public IEnumerable<OrderDetails> Orders { get; set; }

        [JsonPropertyName("paginate")]
        public Paginate Paginate { get; set; }
    }
}
