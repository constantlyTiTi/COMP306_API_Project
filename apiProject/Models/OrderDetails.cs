using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class OrderDetails
    {
        public long OrderId { get; set; }
        public string Status { get; set; }
        public double TotalCost { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderTime { get; set; }
        public string UserName { get; set; }

    }
}
