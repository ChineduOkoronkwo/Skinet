using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using services.Dtos;
using services.Errors;
using services.Extensions;

namespace services.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser() 
        {            
            var email = UserManagerExtensions.GetUserEmail(HttpContext.User);
            var user = await _userManager.FindByEmailAsync(email);
            
            return new UserDto 
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress() 
        {
            var user = await _userManager
                .FindUserWithAddressByClaimsPrincipalAsync(HttpContext.User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address) {
            var user = await _userManager
                .FindUserWithAddressByClaimsPrincipalAsync(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result =  await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                return _mapper.Map<Address, AddressDto>(user.Address);
            }

            return BadRequest("Problem was encountered updating user address");
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery]string email) 
        {
            return await _userManager.FindByEmailAsync(email) != null;           
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) {
                return Unauthorized(new ServiceResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) {
                return Unauthorized(new ServiceResponse(401)); 
            }

            return new UserDto {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value) 
            {
                return BadRequest(new ServicesValidationErrorResponce{
                    Errors = new []{"Email address is in use"}});
            }

            var user = new AppUser 
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                DisplayName = registerDto.DisplayName                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded) {
                return new UserDto 
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user)
                };
            }

            return BadRequest(new ServiceResponse(400));
        }
    }
}