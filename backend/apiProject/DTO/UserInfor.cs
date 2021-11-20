using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.DTO
{
    public class UserInfor
    {
        [StringLength(50)]
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [StringLength(50)]
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("ip_address")]
        public string IpAddress { get; set; } = "";

        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
    }
}
