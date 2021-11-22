using apiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.DTO
{
    public class OrderList
    {
        [JsonPropertyName("orders")]
        public IEnumerable<OrderDetailDTO> Orders { get; set; }

        [JsonPropertyName("paginate")]
        public Paginate Paginate { get; set; }
    }
}
