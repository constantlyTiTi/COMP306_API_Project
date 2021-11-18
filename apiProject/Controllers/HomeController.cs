using apiProject.DBContexts;
using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace apiProject.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MSSQLDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IMapper mapper, MSSQLDbContext db, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public IActionResult login(UserLogin loginUser)
        {
            try
            {
                User user = _unitOfWork.User.GetUser(loginUser.UserName, loginUser.Password).GetAwaiter().GetResult();
                return Ok(user);
            }
            catch (Exception e)
            {
                var model = new ErrorMsg { Error = e.Message };
                return NotFound(model);
            }
        }

    }

}
