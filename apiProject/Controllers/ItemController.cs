using Amazon.S3;
using apiProject.DBContexts;
using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _amazonS3;
        private readonly IUnitOfWork _unitOfWork;

        public ItemController(IMapper mapper,IAmazonS3 amazonS3, IUnitOfWork unitOfWork)
        {
            _amazonS3 = amazonS3;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet("all-item")]
        public IActionResult Get(int items_per_page = 10, string next_cursor = "0")
        {
            var items_all =  _unitOfWork.Item.GetAll();

            Paginate paginate = new Paginate(items_per_page, next_cursor);
            ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
            _mapper.Map(paginate, itemList);

            return Ok(itemList);
        }

        [HttpPost("post-item")]
        public IActionResult PostNewItem(ItemDTO item_form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //save item to db
            item_form.UploadItemDateTime = DateTime.Now;
            Item item = _mapper.Map<Item>(item_form);

            _unitOfWork.Item.Add(item);
            _unitOfWork.Save();

            //save itemFile To db
            for (int i = 1; i< item_form.ItemImages.Count; i++)
            {
                byte[] fileBytes = new Byte[item_form.ItemImages.ElementAt(i-1).Length];
                item_form.ItemImages.ElementAt(i - 1).OpenReadStream().Read(fileBytes, 0, Int32.Parse(item_form.ItemImages.ElementAt(i - 1).Length.ToString()));
                string fileKey = item_form.ItemId + "-" + i+ "." + item_form.ItemImages.ElementAt(i - 1).FileName.Split(".").Last();
                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    _unitOfWork.S3Services.SaveImgs(fileKey, stream);
                }
                ItemFile itemFile = new ItemFile(item_form.ItemId, fileKey);
                _unitOfWork.ItemFile.Add(itemFile);
                _unitOfWork.Save();
            }
            return Ok(item_form);
        }
        [HttpGet("/items")]
        public IActionResult FilterItems(string item_name = "", string postal_code = "", 
            DateTime? upload_date_time = null, string category = "", int items_per_page = 10, string next_cursor = "0")
        {
            if(!string.IsNullOrWhiteSpace(item_name) && !string.IsNullOrWhiteSpace(postal_code))
            {
                var items_all = _unitOfWork.Item.GetItemByItemNamePostalCode(item_name,postal_code).Result;

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(item_name))
            {
                var items_all = _unitOfWork.Item.GetItemByItemName(item_name).Result;

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var items_all = _unitOfWork.Item.GetItemByCategory(category).Result;

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(postal_code))
            {
                var items_all = _unitOfWork.Item.GetItemByLocation(postal_code).Result;

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

                if (upload_date_time != null)
            {
                var items_all = _unitOfWork.Item.GetItemByUploadedDateTime(upload_date_time.Value, upload_date_time.Value.AddDays(1)).Result;

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                ItemList itemList = _mapper.Map<ItemList>(GetItemsPerPage(items_all, items_per_page, next_cursor));
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            var model = new ErrorMsg { Error = "invalid filter condition" };
            return BadRequest(model);
        }

        private List<Item> GetItemsPerPage(IEnumerable<Item> items_all, int items_per_page, string next_cursor)
        {
            List<Item> items = null;
            int totalItems = items_all.Count();
            int startIndex = int.Parse(next_cursor) * 10;
            if (startIndex + items_per_page < totalItems)
            {
                items = items_all.ToList().GetRange(startIndex, items_per_page);
            }
            else
            {
                items = items_all.ToList().GetRange(startIndex, totalItems - startIndex);
            }
            return items;
        }
    }
}
