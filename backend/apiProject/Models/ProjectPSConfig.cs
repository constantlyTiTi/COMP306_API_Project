using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Models
{
    public class ProjectPSConfig
    {
        public const string SectionName = "lab03";
        public string RDSMasterAccount { get; set; }
        public string RDSPassword { get; set; }
        public string AccessKeyID { get; set; }
        public string SecretAccessKey { get; set; }
        public string ConnectionString { get; set; }
        public string JwtSecretToken { get; set; }
    }
}
