using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IS3Services
    {
        void SaveImg(string fileKey, Stream fileStream);
        void DeleteImg(string fileKey);
        Task DeleteImgs(IEnumerable<string> keys);
    }
}
