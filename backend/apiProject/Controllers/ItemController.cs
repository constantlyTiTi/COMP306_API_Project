using Amazon.S3;
using apiProject.DBContexts;
using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProject.Controllers
{
    [Route("api/item")]
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

        [AllowAnonymous]
        [HttpGet("all-item")]
        public IActionResult Get(int items_per_page = 10, string next_cursor = "0")
        {
            IEnumerable<Item> items_all =  _unitOfWork.Item.GetAll();
            IEnumerable<ItemFile> itemFiles_all = _unitOfWork.ItemFile.GetAll();
            IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
            foreach(var dto in itemDtos)
            {
                IEnumerable<ItemFile> itemFiles = itemFiles_all.Where(i => i.ItemId == dto.ItemId);
                _mapper.Map(itemFiles, dto);
            }

            Paginate paginate = new Paginate(items_per_page, next_cursor);
            List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
            if (itemsOfPage == null)
            {
                return NotFound();
            }
            ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
            _mapper.Map(paginate, itemList);

            return Ok(itemList);
        }

        [Authorize]
        [HttpPost("post-item")]
        [RequestSizeLimit(40000000)]
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
                    _unitOfWork.S3Services.SaveImg(fileKey, stream);
                }
                ItemFile itemFile = new ItemFile(item_form.ItemId, @"https://comp306-lab03.s3.amazonaws.com/img/"+fileKey);
                _unitOfWork.ItemFile.Add(itemFile);
                _unitOfWork.Save();
            }
            return Ok(item_form);
        }

        [AllowAnonymous]
        [HttpGet("items")]
        public IActionResult FilterItems(string item_name = "", string postal_code = "", 
            DateTime? upload_date_time = null, string category = "", int items_per_page = 10, string next_cursor = "0")
        {
            if(!string.IsNullOrWhiteSpace(item_name) && !string.IsNullOrWhiteSpace(postal_code))
            {
                var items_all = _unitOfWork.Item.GetItemByItemNamePostalCode(item_name,postal_code).Result;
                IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);

                IEnumerable<long> items_all_ids = items_all.Select(i => i.ItemId);
                IEnumerable<ItemFile> itemFiles_all = _unitOfWork.ItemFile.GetAllItemByIds(items_all_ids).Result;

                foreach (var dto in itemDtos)
                {
                    IEnumerable<ItemFile> itemFiles = itemFiles_all.Where(i => i.ItemId == dto.ItemId);
                    _mapper.Map(itemFiles, dto);
                }

                Paginate paginate = new Paginate(items_per_page, next_cursor);
                List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
                if (itemsOfPage == null)
                {
                    return NotFound();
                }
                ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(item_name))
            {
                var items_all = _unitOfWork.Item.GetItemByItemName(item_name).Result;
                IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
                Paginate paginate = new Paginate(items_per_page, next_cursor);
                List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
                if (itemsOfPage == null)
                {
                    return NotFound();
                }
                ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                var items_all = _unitOfWork.Item.GetItemByCategory(category).Result;
                IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
                Paginate paginate = new Paginate(items_per_page, next_cursor);
                List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
                if (itemsOfPage == null)
                {
                    return NotFound();
                }
                ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            if (!string.IsNullOrWhiteSpace(postal_code))
            {
                var items_all = _unitOfWork.Item.GetItemByLocation(postal_code).Result;
                IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
                Paginate paginate = new Paginate(items_per_page, next_cursor);
                List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
                if (itemsOfPage == null)
                {
                    return NotFound();
                }
                ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

                if (upload_date_time != null)
            {
                var items_all = _unitOfWork.Item.GetItemByUploadedDateTime(upload_date_time.Value, upload_date_time.Value.AddDays(1)).Result;
                IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
                Paginate paginate = new Paginate(items_per_page, next_cursor);
                List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
                if (itemsOfPage == null)
                {
                    return NotFound();
                }
                ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
                _mapper.Map(paginate, itemList);

                return Ok(itemList);
            }

            var model = new ErrorMsg { Error = "invalid filter condition" };
            return BadRequest(model);
        }

        [Authorize]
        [HttpGet("items/{uploaderusername}")]
        public IActionResult FilterItemsByUploader(string uploaderusername, int items_per_page = 10, string next_cursor = "0")
        {
            var items_all = _unitOfWork.Item.GetItemByUserName(uploaderusername).Result;
            IEnumerable<ItemDTO> itemDtos = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDTO>>(items_all);
            Paginate paginate = new Paginate(items_per_page, next_cursor);
            List<ItemDTO> itemsOfPage = GetItemsPerPage(itemDtos, items_per_page, next_cursor);
            if(itemsOfPage == null)
            {
                return NotFound();
            }

            IEnumerable<long> items_all_ids = items_all.Select(i => i.ItemId);
            IEnumerable<ItemFile> itemFiles_all = _unitOfWork.ItemFile.GetAllItemByIds(items_all_ids).Result;

            foreach (var dto in itemDtos)
            {
                IEnumerable<ItemFile> itemFiles = itemFiles_all.Where(i => i.ItemId == dto.ItemId);
                _mapper.Map(itemFiles, dto);
            }

            ItemList itemList = _mapper.Map<ItemList>(itemsOfPage);
            _mapper.Map(paginate, itemList);

            return Ok(itemList);
        }

        [AllowAnonymous]
        [HttpGet("{itemid}")]
        public IActionResult FilterItemsByItemId( long itemid)
        {
            Item item = _unitOfWork.Item.Get(itemid);
            if(item == null)
            {
                return NotFound();
            }
            var itemFiles = _unitOfWork.ItemFile.GetItemByItemId(itemid).Result;
            ItemDTO itemDTO = _mapper.Map<ItemDTO>(itemFiles);
            _mapper.Map(item, itemDTO);

            return Ok(itemDTO);
        }

        [Authorize]
        [HttpPut("{uploaderusername}/{itemid}")]
        public IActionResult UpdateItem(string uploaderusername, long itemid, ItemDTO itemDTO)
        {
            Item item = _unitOfWork.Item.Get(itemid);

            if (item.UserName != uploaderusername)
            {
                var model = new ErrorMsg { Error = "The item is not uploaded by you" };
                return BadRequest(model);
            }

            item = _mapper.Map<Item>(itemDTO);
            if(itemDTO.ItemId == 0)
            {
                item.ItemId = itemid;
            }

            _unitOfWork.Item.UpdateItem(item);

            int newFileStartIndex = 0;

            if (_unitOfWork.ItemFile.GetItemByItemId(itemid).Result.Count() > 0)
            {
                newFileStartIndex = int.Parse(_unitOfWork.ItemFile.GetItemByItemId(itemid)
                .Result.Last().ImgFileKey.Split(".").First().Split("-").Last());
            }
            
            int realIndex = 0;
            foreach(var img in itemDTO.ItemImages)
            {
                newFileStartIndex++;
                byte[] fileBytes = new Byte[itemDTO.ItemImages.ElementAt(realIndex).Length];
                itemDTO.ItemImages.ElementAt(realIndex).OpenReadStream().Read(fileBytes, 0, Int32.Parse(itemDTO.ItemImages.ElementAt(realIndex).Length.ToString()));
                string fileKey = itemDTO.ItemId + "-" + newFileStartIndex + "." + itemDTO.ItemImages.ElementAt(realIndex).FileName.Split(".").Last();
                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    _unitOfWork.S3Services.SaveImg(fileKey, stream);
                }
                ItemFile itemFile = new ItemFile(itemDTO.ItemId, ResourceUrl.ImgBucket.ToUrl() + fileKey);
                _unitOfWork.ItemFile.Add(itemFile);
                _unitOfWork.Save();
                realIndex++;
            }



            return Ok(itemDTO);
        }

        [Authorize]
        [HttpDelete("{uploaderusername}/{itemid}")]
        public IActionResult DeleteItem(string uploaderusername, long itemid)
        {
            Item item = _unitOfWork.Item.Get(itemid);

            if (item.UserName != uploaderusername)
            {
                var model = new ErrorMsg { Error = "The item is not uploaded by you" };
                return BadRequest(model);
            }

            _unitOfWork.Item.Remove(itemid);
            _unitOfWork.Save();

            var itemFilesById = _unitOfWork.ItemFile.GetItemByItemId(itemid).Result;

            if (itemFilesById.Count() > 0)
            {
                var itemFiles = itemFilesById.Select(i => i.ImgFileKey);
                try
                {
                    _unitOfWork.S3Services.DeleteImgs(itemFiles);
                }
                catch (Exception e)
                {
                    var model = new ErrorMsg { Error = "Something wrong with the image folder deletion" };
                    return BadRequest(model);
                }
                _unitOfWork.ItemFile.RemoveByItemId(itemid);
            }

            return Ok();
        }

        private List<ItemDTO> GetItemsPerPage(IEnumerable<ItemDTO> items_all, int items_per_page, string next_cursor)
        {
            List<ItemDTO> items = null;
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
