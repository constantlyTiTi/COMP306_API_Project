using Amazon.DynamoDBv2.DataModel;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("OrderItem")]
    public class OrderItem
    {
        [PrimaryKey]
        public long OrderItemId { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Quantity { get; set; } = 1;

    }
}
