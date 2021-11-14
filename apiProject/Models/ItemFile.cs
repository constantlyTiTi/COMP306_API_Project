using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiProject.Models
{
    [Table("API_Project")]
    public class ItemFile
    {
        [DynamoDBHashKey]
        public long ItemId { get; set; }
        public List<string> ImgFileKeys { get; set; }
        public List<string> ImgDescriptions { get; set; }
        [Required]
        public string LocationPostalCode { get; set; }

    }
}
