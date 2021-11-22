using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    [Route("api/shopping_cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("{itemid}")]
        [AllowAnonymous]
        public IActionResult PostItemToCart([FromBody] ShoppingCartItem cartItem)
        {
            bool isCartCreated = HttpContext.Session.Keys.Any(i => i == "Cart");
            List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();

            if (isCartCreated)
            {
                var cartInfor = HttpContext.Session.GetString("Cart");
                cartItems = (List<ShoppingCartItem>)JsonConvert.DeserializeObject(cartInfor);
            }

            cartItems.Add(cartItem);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                _unitOfWork.ShoppingCartItems.Add(cartItem);
            }

            ShoppingCartDTO cartDTO = _mapper.Map<ShoppingCartDTO>(cartItems);
            IEnumerable<Item> items = _unitOfWork.Item.GetAllByIds(cartItems.Select(i => i.ItemId)).Result;
            IEnumerable<ItemDTO> itemDTOs = _mapper.Map<IEnumerable<ItemDTO>>(items);
            _mapper.Map(cartItems, itemDTOs);
            _mapper.Map(itemDTOs, cartDTO);

            return Ok(cartDTO);
        }

        [AllowAnonymous]
        [HttpDelete("{itemid}")]
        public IActionResult DeleteCartItem(long item_id)
        {
            var cartInfor = HttpContext.Session.GetString("Cart");
            List<ShoppingCartItem> cartItems = (List<ShoppingCartItem>)JsonConvert.DeserializeObject(cartInfor);
            var item = cartItems.FirstOrDefault(i => i.ItemId == item_id);
            cartItems.Remove(item);
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cartItems));

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                _unitOfWork.ShoppingCartItems.Remove(item);
            }

            ShoppingCartDTO cartDTO = _mapper.Map<ShoppingCartDTO>(cartItems);
            IEnumerable<Item> items = _unitOfWork.Item.GetAllByIds(cartItems.Select(i => i.ItemId)).Result;
            IEnumerable<ItemDTO> itemDTOs = _mapper.Map<IEnumerable<ItemDTO>>(items);
            _mapper.Map(cartItems, itemDTOs);
            _mapper.Map(itemDTOs, cartDTO);

            return Ok(cartDTO);
        }

        [AllowAnonymous]
        [HttpGet("all-items")]
        public IActionResult GetShoppingCartItems()
        {
            List<ShoppingCartItem> cartItems = new List<ShoppingCartItem>();
            ShoppingCartDTO cartDTO = new ShoppingCartDTO();
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                cartItems = _unitOfWork.ShoppingCartItems.GetItems(User.Identity.Name).ToList();

                _mapper.Map(cartItems, cartDTO);
                IEnumerable<Item> items = _unitOfWork.Item.GetAllByIds(cartItems.Select(i => i.ItemId)).Result;
                IEnumerable<ItemDTO> itemDTOs = _mapper.Map<IEnumerable<ItemDTO>>(items);
                _mapper.Map(cartItems, itemDTOs);
                _mapper.Map(itemDTOs, cartDTO);
                return Ok(cartDTO);

            }

            bool isCartCreated = HttpContext.Session.Keys.Any(i => i == "Cart");
            if (isCartCreated)
            {
                var cartInfor = HttpContext.Session.GetString("Cart");
                cartItems = (List<ShoppingCartItem>)JsonConvert.DeserializeObject(cartInfor);
            }

            _mapper.Map(cartItems, cartDTO);
            return Ok(cartDTO);
        }

        [Authorize]
        [HttpPost("place-order")]
        public IActionResult PlaceOrder([FromBody] ShoppingCartDTO cartDTO)
        {
            List<ShoppingCartItem> cartItems = _unitOfWork.ShoppingCartItems.GetItems(cartDTO.UserName).ToList();
            var order = _mapper.Map<OrderDetails>(cartItems);
            _unitOfWork.OrderDetails.Add(order);
            foreach (var item in cartItems)
            {
                var orderItem = _mapper.Map<OrderItem>(item);
                _unitOfWork.OrderItem.Add(_mapper.Map(order, orderItem));
            }

            _unitOfWork.ShoppingCartItems.RemoveAll(cartDTO.UserName);

            return Ok(_mapper.Map(cartItems, cartDTO));

        }
    }
}
