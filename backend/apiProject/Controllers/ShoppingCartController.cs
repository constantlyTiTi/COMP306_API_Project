using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace apiProject.Controllers
{
    [Route("api/shopping_cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private static Dictionary<long,List<ShoppingCartItem>> CartItems = new Dictionary<long, List<ShoppingCartItem>>();
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;

        public ShoppingCartController(IMapper mapper, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("generate-unique_tempor_user_id")]
        [AllowAnonymous]
        public long GenerateUniqueKey()
        {
            Random rand = new Random();
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            while (CartItems.ContainsKey(longRand))
            {
                rand.NextBytes(buf);
                longRand = BitConverter.ToInt64(buf, 0);
            }

            return longRand;
        }

        [HttpPost("{itemid}/{unique_tempor_user_id}")]
        [AllowAnonymous]
        public IActionResult PostItemToCart([FromBody] ShoppingCartItem cartItem, long unique_tempor_user_id)
        {
            if(CartItems.ContainsKey(unique_tempor_user_id))
            {
                if (CartItems[unique_tempor_user_id].Any(i => i.ItemId == cartItem.ItemId))
                {
                    return BadRequest(new ErrorMsg
                    {
                        Error = "This item has already exist in the shopping cart, " +
                        "if you want to by more, please go to shopping cart and edit the quantity"
                    });
                }
            }
            if (CartItems.ContainsKey(unique_tempor_user_id))
            {

               CartItems[unique_tempor_user_id].Add(cartItem);

            }
            else
            {
                CartItems.Add(unique_tempor_user_id, new List<ShoppingCartItem> { cartItem });
            }

            

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                _unitOfWork.ShoppingCartItems.Add(cartItem);
            }

            return Ok(MapCartDTO(unique_tempor_user_id));
        }

        private ShoppingCartDTO MapCartDTO (long unique_tempor_user_id)
        {
            ShoppingCartDTO cartDTO = _mapper.Map<ShoppingCartDTO>(CartItems[unique_tempor_user_id]);
            IEnumerable<Item> items = _unitOfWork.Item.GetAllByIds(CartItems[unique_tempor_user_id].Select(i => i.ItemId)).Result;
            IEnumerable<ItemFile> itemFiles = _unitOfWork.ItemFile.GetAllItemByIds(CartItems[unique_tempor_user_id].Select(i => i.ItemId)).Result;
            IEnumerable<ItemDTO> itemDTOs = _mapper.Map<IEnumerable<ItemDTO>>(items);
            foreach (var item in itemDTOs)
            {
                _mapper.Map(CartItems[unique_tempor_user_id].FirstOrDefault(i => i.ItemId == item.ItemId), item);
                _mapper.Map(itemFiles.Where(i => i.ItemId == item.ItemId), item);
            }

            _mapper.Map(itemDTOs, cartDTO);
            return cartDTO;
        }

        [AllowAnonymous]
        [HttpDelete("{item_id}/{unique_tempor_user_id}")]
        public IActionResult DeleteCartItem(long item_id, long unique_tempor_user_id)
        {

            var item = CartItems[unique_tempor_user_id].FirstOrDefault(i => i.ItemId == item_id);
            CartItems[unique_tempor_user_id].Remove(item);

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                _unitOfWork.ShoppingCartItems.Remove(item);
            }

            return Ok(MapCartDTO(unique_tempor_user_id));
        }
        [AllowAnonymous]
        [HttpDelete("clear-temp-cart/{unique_tempor_user_id}")]
        public IActionResult ClearCart(long unique_tempor_user_id)
        {
            CartItems.Remove(unique_tempor_user_id);
            return Ok();
        }


        [AllowAnonymous]
        [HttpGet("all-items/{unique_tempor_user_id}")]
        public IActionResult GetShoppingCartItems(long unique_tempor_user_id)
        {

            ShoppingCartDTO cartDTO = new ShoppingCartDTO();
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                string token = HttpContext.Request.Headers["Authorization"];
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                string name = jwtTokenHandler.ReadJwtToken(token.Split(" ")[1]).Subject;
                List<ShoppingCartItem> dbList = _unitOfWork.ShoppingCartItems.GetItems(name).ToList();
                if (dbList.Any() && !CartItems.ContainsKey(unique_tempor_user_id))
                {
                    CartItems.Add(unique_tempor_user_id, dbList);
                }
                if (CartItems.ContainsKey(unique_tempor_user_id))
                {
                    List<long> tempItemIds = CartItems[unique_tempor_user_id].Select(i => i.ItemId).ToList();
                    List<long> dbItemIds = dbList.Select(i => i.ItemId).ToList();
                    CartItems[unique_tempor_user_id].Concat(dbList.Where(i => !tempItemIds.Contains(i.ItemId)));
                    foreach(var i in CartItems[unique_tempor_user_id].Where(i=> !dbItemIds.Contains(i.ItemId)))
                    {
                        i.UserName = name;
                        _unitOfWork.ShoppingCartItems.Add(i);
                        _unitOfWork.Save();
                    }
                }

            }

            if (!CartItems.ContainsKey(unique_tempor_user_id))
            {
                return Ok(cartDTO);
            }
            if (CartItems[unique_tempor_user_id].Count == 0)
            {
                return Ok(cartDTO);
            }
            return Ok(MapCartDTO(unique_tempor_user_id));
        }

        [Authorize]
        [HttpPost("place-order")]
        public IActionResult PlaceOrder(string user_name, [FromBody]ShoppingCartDTO cart)
        {
            IEnumerable<ShoppingCartItem> cartItems = _unitOfWork.ShoppingCartItems.GetItems(user_name);
            _mapper.Map(cartItems, cart);
            var order = _mapper.Map<OrderDetails>(cart);
            _unitOfWork.OrderDetails.Add(order);
            _unitOfWork.Save(); 

            order.OrderId = _unitOfWork.OrderDetails.GetFirstOrDefault(i=>i.OrderTime == order.OrderTime && i.UserName == user_name).OrderId;

            foreach (var item in cartItems)
            {
                var orderItem = _mapper.Map<OrderItem>(item);
                _unitOfWork.OrderItem.Add(_mapper.Map(order, orderItem));
            }
            _unitOfWork.Save();

            foreach (var item in cartItems)
            {
                _unitOfWork.ShoppingCartItems.DeleteItemWithoutSave(item.ItemId, user_name);

            }
            _unitOfWork.Save();
            return Ok();

        }

        [AllowAnonymous]
        [HttpPut("update-cart/{unique_tempor_user_id}")]
        public IActionResult UpdateCart([FromBody] ShoppingCartItem shoppingCartItem, long unique_tempor_user_id)
        {

            int index = CartItems[unique_tempor_user_id].FindIndex(i => i.ItemId == shoppingCartItem.ItemId);
            CartItems[unique_tempor_user_id][index] = shoppingCartItem;

            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                _unitOfWork.ShoppingCartItems
                    .UpdateItemQuantity(shoppingCartItem.ItemId, shoppingCartItem.Quantity, shoppingCartItem.UserName);
            }

            return Ok(MapCartDTO(unique_tempor_user_id));
        }
    }
}
