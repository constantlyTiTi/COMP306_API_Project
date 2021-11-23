using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public long OrderId { get; set; }
        public string Status { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderTime { get; set; }
        public string UserName { get; set; }

    }
}
