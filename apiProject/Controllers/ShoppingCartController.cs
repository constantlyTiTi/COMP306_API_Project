using apiProject.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    [Route("shopping_cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartController(IMapper mapper,  IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("{itemid}")]
        public IActionResult PostItemToCart(long itemid)
        {         
            bool isCartCreated = HttpContext.Session.Keys.Any(i=>i == "Cart");

            if (!isCartCreated)
            {
                
            }

            return Ok();
        }
    }
}
