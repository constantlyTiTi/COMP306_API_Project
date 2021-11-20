using apiProject.DTO;
using apiProject.DTO.Responses;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.TokenAuth;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiProject.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ProjectPSConfig _psConfig;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenManager _tokenManger;
         
        public HomeController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, 
            IMapper mapper, IOptions<ProjectPSConfig> options, UserManager<IdentityUser> userManager, ITokenManager tokenManger)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _mapper = mapper;
            _psConfig = options.Value;
            _userManager = userManager;
            _tokenManger = tokenManger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] UserInfor loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(loginUser.UserName);
            if (existingUser == null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>()
                    {
                        "User name does not exsit"
                    },
                    Success = false
                });
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, loginUser.Password);

            if (!isCorrect)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>()
                    {
                        "Password does not match the record"
                    },
                    Success = false
                });
            }

            var jwtToken = _tokenManger.GenerateJwtToken(existingUser, _psConfig);

            User user = _unitOfWork.User.GetUser(loginUser.UserName).GetAwaiter().GetResult();
            loginUser = _mapper.Map<UserInfor>(user);
            loginUser.Token = jwtToken;
            return Ok(loginUser);


        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserInfor resgiterUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(resgiterUser.UserName);
            if(existingUser != null)
            {
                return BadRequest(new RegistrationResponse()
                {
                    Errors = new List<string>()
                    {
                        "User name has already been used"
                    },
                    Success = false
                });
            }
            var newUser = new IdentityUser() { UserName = resgiterUser.UserName };
            var isCreated = await _userManager.CreateAsync(newUser, resgiterUser.Password);

            if (isCreated.Succeeded)
            {
                User user = _mapper.Map<User>(newUser);
                _unitOfWork.User.AddUser(user);
                var jwtToken = _tokenManger.GenerateJwtToken(existingUser, _psConfig);

                resgiterUser.Token = jwtToken;

                return Ok(resgiterUser);
            }
            else
            {
                List<string> errors = new List<string>();
                errors = isCreated.Errors.Select(e => e.Description).ToList();
                
                return BadRequest(new RegistrationResponse()
                {
                    Errors = errors,
                    Success = false
                });
            }

        }

        /*private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_psConfig.JwtSecretToken);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    //when previous token expiry, jti will create new token
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                //define algorithsm to create token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;

        }*/


    }

}
