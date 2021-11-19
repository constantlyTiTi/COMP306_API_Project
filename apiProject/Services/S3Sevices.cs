using Amazon.Runtime.Internal.Transform;
using Amazon.S3;
using Amazon.S3.Model;
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

        public void SaveImg(string fileKey, Stream fileStream)
        {
            string fileKey_F = @"/img/" + fileKey;
            IDictionary<string, Object> properties = new Dictionary<string, Object>() { { "CannedACL",S3CannedACL.PublicRead } };
            _amazonS3.UploadObjectFromStreamAsync(bukectName, fileKey_F, fileStream, properties);
        }

        public void DeleteImg(string fileKey)
        {
            string fileKey_F = @"/img/" + fileKey;
            _amazonS3.DeleteObjectAsync(bukectName, fileKey_F);
        }

        public async Task DeleteImgs(IEnumerable<string> keys)
        {
            // a. multi-object delete by specifying the key names and version IDs.
            DeleteObjectsRequest multiObjectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName = bukectName,
                Objects = (List<KeyVersion>)keys // This includes the object keys and null version IDs.
            };
            // You can add specific object key to the delete request using the .AddKey.
            // multiObjectDeleteRequest.AddKey("TickerReference.csv", null);

            DeleteObjectsResponse response = await _amazonS3.DeleteObjectsAsync(multiObjectDeleteRequest);

        }
    }
}
