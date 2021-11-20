using apiProject.DBContexts;
using apiProject.DTO;
using apiProject.Interfaces;
using apiProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
         
        public HomeController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public IActionResult login(UserInfor loginUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                User user = _unitOfWork.User.GetUser(loginUser.UserName, loginUser.Password).GetAwaiter().GetResult();
                loginUser = _mapper.Map<UserInfor>(user);
                return Ok(loginUser);
/*                var result = await _signInManager.PasswordSignInAsync(loginUser.UserName, loginUser.Password, true, true);
                if (result.Succeeded)
                {
                    return Ok(loginUser);
                }*/
/*                else
                {
                    var model = new ErrorMsg { Error = "Cannot find the user" };
                    return NotFound(model);
                }*/

            }
            catch (Exception e)
            {
                var model = new ErrorMsg { Error = e.Message };
                return BadRequest(model);
            }
        }

        [HttpPost("Register")]
        public IActionResult Register(UserInfor resgiterUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = _mapper.Map<User>(resgiterUser);
            _unitOfWork.User.AddUser(user);
            return Ok(user);
        }

    }

}
