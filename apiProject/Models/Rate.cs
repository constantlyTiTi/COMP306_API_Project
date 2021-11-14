using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class Rate
    {
        public long RateId { get; set; }
        public long OrderId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public int RateScore { get; set; }
        public DateTime RateTime { get; set; }
        public long ItemId { get; set; }
    }
}
