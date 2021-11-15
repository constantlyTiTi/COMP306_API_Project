using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    public class ItemController : Controller
    {
        private IAmazonS3 _s3Client;
        private IAmazonDynamoDB _dynamoDbClient;
        private DynamoDBContext _context;
        public ItemController(IAmazonS3 s3Client, IAmazonDynamoDB dynamoDbClient)
        {
            _s3Client = s3Client;
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(_dynamoDbClient);

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
