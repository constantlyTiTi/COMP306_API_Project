using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiProject.Models
{
    //this model is to save item related image file path and each image may have individual description
    [Table("ItemFile")]
    public class ItemFile
    {
        public long ItemFileId { get; set; }
        public long ItemId { get; set; }
        public string ImgFileKey { get; set; }
        public string ImgDescription { get; set; }

    }
}
