using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace apiProject.DTO
{
    public class ItemDTO
    {
        public long ItemId { get; set; }
        [JsonPropertyName("uploader")]
        public string UserName { get; set; }
        [JsonPropertyName("upload_item_date_time")]
        public DateTime UploadItemDateTime { get; set; }
        [Required]
        [JsonPropertyName("item_name")]
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        [JsonPropertyName("cover_Image_path")]
        public string CoverImagePath { get; set; }

        [Required]
        [JsonPropertyName("location_postal_code")]
        public string LocationPostalCode { get; set; }
        [JsonPropertyName("item_imgs")]
        public ICollection<IFormFile> ItemImages { get; set; }

        [JsonPropertyName("item_imgs_paths")]
        public IEnumerable<string> ItemImagePaths { get; set; }
        public string Quantity { get; set; }
    }

}
