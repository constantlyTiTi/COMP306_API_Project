using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiFrontEnd.Models
{
    public class Item
    {
        public long ItemId { get; set; }
        [JsonPropertyName("uploader")]
        public string UserName { get; set; }
        [JsonPropertyName("upload_item_date_time")]
        public DateTime UploadItemDateTime { get; set; }
        [Required]
        [JsonPropertyName("item_name")]
        public string ItemName { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        [JsonPropertyName("cover_Image_path")]
        public string CoverImagePath { get; set; }

        [Required]
        [JsonPropertyName("location_postal_code")]
        public string LocationPostalCode { get; set; }
        [JsonPropertyName("item_imgs")]
        public ICollection<FileStream> ItemImages { get; set; }

        [JsonPropertyName("item_imgs_paths")]
        public IEnumerable<string> ItemImagePaths { get; set; }
    }
}
