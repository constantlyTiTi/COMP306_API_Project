using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("APIProjectUser")]
    public class User
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
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
