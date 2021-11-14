using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class Item
    {
        [PrimaryKey]
        public long ItemId { get; set; }
        [ForeignKey(typeof(User))]
        public string UserName { get; set; }
        public DateTime UploadItemDateTime { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string ItemCover { get; set; }
        public string Description { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public double Price { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string LocationPostalCode { get; set; }
    }
}
