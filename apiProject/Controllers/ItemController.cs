using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    using Amazon.DynamoDBv2.DataModel;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    public class ItemController : Controller
    {
        private IAmazonS3 _s3Client;
        private IAmazonDynamoDB _dynamoDbClient;
        public ItemController(IAmazonS3 s3Client, IAmazonDynamoDB dynamoDbClient)
        {
            _s3Client = s3Client;
            _dynamoDbClient = dynamoDbClient;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
