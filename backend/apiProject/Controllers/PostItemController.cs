using Amazon.S3;
using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    [Route("api/item/post-item")]
    [ApiController]
    public class PostItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _amazonS3;
        private readonly IUnitOfWork _unitOfWork;
        public PostItemController(IMapper mapper, IAmazonS3 amazonS3, IUnitOfWork unitOfWork)
        {
            _amazonS3 = amazonS3;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            for (int i = 1; i < item_form.ItemImages.Count; i++)
            {
                byte[] fileBytes = new Byte[item_form.ItemImages.ElementAt(i - 1).Length];
                item_form.ItemImages.ElementAt(i - 1).OpenReadStream().Read(fileBytes, 0, Int32.Parse(item_form.ItemImages.ElementAt(i - 1).Length.ToString()));
                string fileKey = item_form.ItemId + "-" + i + "." + item_form.ItemImages.ElementAt(i - 1).FileName.Split(".").Last();
                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    _unitOfWork.S3Services.SaveImg(fileKey, stream);
                }
                ItemFile itemFile = new ItemFile(item_form.ItemId, @"https://comp306-lab03.s3.amazonaws.com/img/" + fileKey);
                _unitOfWork.ItemFile.Add(itemFile);
                _unitOfWork.Save();
            }
            return Ok(item_form);
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
            if (itemDTO.ItemId == 0)
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
            foreach (var img in itemDTO.ItemImages)
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


        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
