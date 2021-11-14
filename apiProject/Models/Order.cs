using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class Order
    {
        public long ItemId { get; set; }
        public DateTime OrderTime { get; set; }
        public long OrderId { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }

    }
}
