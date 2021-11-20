using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject
{
    public enum ResourceUrl
    {
        ImgBucket,
        VideoBucket

    }

    public static class ResourceUrlExtensions
    {
        public static string ToUrl(this ResourceUrl ru)
        {
            switch (ru)
            {
                case ResourceUrl.ImgBucket:
                    return @"https://comp306-lab03.s3.amazonaws.com/img/";
                case ResourceUrl.VideoBucket:
                    return @"https://comp306-lab03.s3.amazonaws.com/movie/";
                default:
                    throw new NullReferenceException();
            }
        }
    }
}
