using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using AutoMapper;
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
    using AutoMapper;
    public class ItemController : Controller
    {
        private IAmazonS3 _s3Client;
        private IAmazonDynamoDB _dynamoDbClient;
        private DynamoDBContext _context;
        private readonly IMapper _mapper;
        public ItemController(IAmazonS3 s3Client, IAmazonDynamoDB dynamoDbClient, IMapper mapper)
        {
            _s3Client = s3Client;
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(_dynamoDbClient);
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
