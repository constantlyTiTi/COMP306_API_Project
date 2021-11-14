using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class OrderItem
    {
        public long ItemId { get; set; }
        public long OrderId { get; set; }


    }
}
