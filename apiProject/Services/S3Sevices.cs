using Amazon.Runtime.Internal.Transform;
using Amazon.S3;
using apiProject.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.DAL
{
    public class S3Sevices: IS3Services
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly string bukectName = "comp306-lab03";
        public S3Sevices(IAmazonS3 amazonS3)
        {
            _amazonS3 = amazonS3;
        }

        public void SaveImgs(string fileKey, Stream fileStream)
        {
            string fileKey_F = @"/img/" + fileKey;
            IDictionary<string, Object> properties = new Dictionary<string, Object>() { { "CannedACL",S3CannedACL.PublicRead } };
            _amazonS3.UploadObjectFromStreamAsync(bukectName, fileKey_F, fileStream, properties);
        }

        public void DeleteImgs(string fileKey)
        {
            string fileKey_F = @"/img/" + fileKey;
            _amazonS3.DeleteObjectAsync(bukectName, fileKey_F);
        }
    }
}
