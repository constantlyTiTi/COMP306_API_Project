using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiProject.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        /*private readonly UserManager<User> _userManager;*/
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(ShoppingCartDTO shoppingCart)
        {
            await _unitOfWork.ShoppingCartItems.RemoveAll(shoppingCart.UserName);
            OrderDetails Order = _mapper.Map<OrderDetails>(shoppingCart);
            _unitOfWork.OrderDetails.Add(Order);
            OrderDetails addedOrder = _unitOfWork.OrderDetails.GetAllOrdersByUserName(Order.UserName).Result.Last();
            return Ok(addedOrder);
        }

        [HttpGet("{username}")]
        public IActionResult GetOrders(string username, DateTime? start_date = null, DateTime? end_date = null, 
            int items_per_page = 10, string next_cursor = "0")
        {
            Paginate paginate = new Paginate(items_per_page, next_cursor);
            OrderList orderList = _mapper.Map<OrderList>(paginate);
            IEnumerable<OrderDetails> orders = _unitOfWork.OrderDetails.GetAllOrdersByDateTime(start_date, end_date, username).Result;
            _mapper.Map(orders, orderList);
            return Ok(orderList);
        }

        [HttpDelete("{username}")]
        public IActionResult DeleteOrder(string username, long order_id)
        {
            if(_unitOfWork.OrderDetails.Get(order_id).UserName != username)
            {
                var model = new ErrorMsg { Error = "You are not allow to modify this order" };
                return BadRequest(model);
            }
            _unitOfWork.OrderDetails.Remove(order_id);
            return Ok();
        }

    }
}
