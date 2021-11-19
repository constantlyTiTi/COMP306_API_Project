using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Interfaces
{
    public interface IS3Services
    {
        void SaveImgs(string fileKey, Stream fileStream);
        void DeleteImgs(string fileKey);
    }
}
