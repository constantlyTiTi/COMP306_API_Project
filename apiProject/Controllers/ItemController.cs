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
        public IActionResult Get(int total = 10, string next_cursor = "0")
        {
            var items_all =  _unitOfWork.Item.GetAll();
            List<Item> items = null;
            if (!items_all.Any())
            {
                var model = new ErrorMsg { Error = "no item is found" };
                return NotFound(model);
            }
            int totalItems = items_all.Count();
            int startIndex = int.Parse(next_cursor) * 10;
            if(startIndex + total < totalItems)
            {
                items = items_all.ToList().GetRange(startIndex, total);
            }
            else
            {
                items = items_all.ToList().GetRange(startIndex, totalItems - startIndex);
            }

            Paginate paginate = new Paginate(total, next_cursor);
            ItemList itemList = _mapper.Map<ItemList>(items);
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

        // POST api/<ItemController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
