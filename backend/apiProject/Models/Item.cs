using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("Item")]
    public class Item
    {
        [PrimaryKey]
        public long ItemId { get; set; }
        [ServiceStack.DataAnnotations.ForeignKey(typeof(User))]
        public string UserName { get; set; }
        public DateTime UploadItemDateTime { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public double Price { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string LocationPostalCode { get; set; }
    }
}
