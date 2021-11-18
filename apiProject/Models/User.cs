using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("APIProjectUser")]
    public class User
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        [StringLength(50)]
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [StringLength(50)]
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; }

        [NotMapped]
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /*        [DataType(DataType.Date)]
                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                public DateTime LoginTime { get; set; }*/
    }
}
