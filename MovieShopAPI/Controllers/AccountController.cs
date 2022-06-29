using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using ApplicationCore.Contracts.Repository;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountService accountService, IUserRepository userRepository, IConfiguration configuration)
        {
            _accountService = accountService;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register( [FromBody] UserRegisterModel model)
        {
            var user = await _accountService.RegisterUser(model);

            return Ok(user);
        }


        [HttpGet]
        [Route("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if(user == null)
            {
                return NotFound(new { errorMessage = "Email does not exist." });
            }
            return Ok(user);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model.Email, model.Password);

            if (user != null)
            {
                // create token
                var jwtToken = CreateJwtToken(user);
                return Ok(new { token = jwtToken });
            }
            // iOS (), Android, Angular, React, Java
            // token, JWT (Json Web Token)

            // Client will send email/password to API, POST 
            // API will validate the email/password and if valid then API will create the JWT token and send to client
            // Its Client's responsibility to store the token some where
            // Angular, React (localstorage or sessionstorage in browser)
            // iOS/Android, device
            // when client needs some secure information or needs to perform any operation that requires users to be 
            // authenticated then client needs to send the token to the API in the Http Header
            // Once the API recieves the token from client it will validate the JWT token and if vlaid it will send the data back to the client
            // IF JWT token is in valid or token is expired then API will send 401 Unauthorized
            throw new UnauthorizedAccessException("Please check email and password");
            // return Unauthorized(new { errorMessage = "Please check email and password" });

        }

        private string CreateJwtToken(UserModel user)
        {
            // create the claims 

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim("Country", "USA"),
                new Claim("language", "english")
            };

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            // specify a secret key
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));

            // specify the algorithm
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            //specify the expiration of the token
            var tokenExpiration = DateTime.UtcNow.AddHours(2);

            // create and object with all the above information so create the token
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Clients"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt = tokenHandler.CreateToken(tokenDetails);
            return tokenHandler.WriteToken(encodedJwt);
        }


    }
}
