using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{

    public class User
    {
        [StringLength(50)]
        [Required]
        public string UserName { get; set; }
        [StringLength(50)]
        [Required]
        public string Password { get; set; }
        public string IpAddress { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LoginTime { get; set; }
    }
}
