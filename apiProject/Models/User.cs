using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace apiProject.Models
{
    [Table("APIProjectUser")]
    public class User
    {
        [ServiceStack.DataAnnotations.PrimaryKey]
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] IpAddress { get; set; } = null;

        /*        [DataType(DataType.Date)]
                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                public DateTime LoginTime { get; set; }*/
    }
}
