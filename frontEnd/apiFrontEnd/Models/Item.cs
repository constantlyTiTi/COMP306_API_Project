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
/*        [JsonPropertyName("upload_item_date_time")]*/
        public DateTime upload_item_date_time { get; set; }
        [Required]
/*        [JsonPropertyName("item_name")]*/
        public string item_name { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

/*        [JsonPropertyName("cover_Image_path")]*/
        public string cover_Image_path { get; set; }

        [Required]
/*        [JsonPropertyName("location_postal_code")]*/
        public string location_postal_code { get; set; }
/*        [JsonPropertyName("item_imgs")]*/
        public ICollection<FileStream> item_imgs { get; set; }

/*        [JsonPropertyName("item_imgs_paths")]*/
        public IEnumerable<string> item_imgs_paths { get; set; }
        public int Quantity { get; set; }
    }
}
