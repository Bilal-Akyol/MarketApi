using MarketApi.Extensions;
using MarketBusiness.Abstract;
using MarketEntity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly Jwt _jwtAyarlari;

        public AuthController(IAuthService authService, IOptions<Jwt> jwtAyarlari)
        {
            _authService = authService;
            _jwtAyarlari = jwtAyarlari.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [SwaggerOperation(Summary = "User ve Admin ortak giriş")]
        public UserLoginResponse Login(UserLoginRequest request)
        {
            var response = _authService.Login(request);

            if (response.Code == "200")
            {
                response.Token = GetToken(request.isRemember, response.UserId, response.RoleId);
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        [SwaggerOperation(Summary = "User kayıt ol")]
        public UserRegisterResponse Register(UserRegisterRequest request)
        {
            request.Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
            return _authService.Register(request);
        }

        [HttpPost]
        [Route("Logout")]
        [SwaggerOperation(Summary = "Logout")]
        public LogoutResponse Logout()
        {
            var request = new LogoutRequest();

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var sessionToken = User.Claims.FirstOrDefault(c => c.Type == "sessionToken")?.Value;

            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            request.SessionToken = sessionToken;

            return _authService.Logout(request);
        }

        private string GetToken(bool isRemember, long id, long roleId)
        {
            if (string.IsNullOrWhiteSpace(_jwtAyarlari.Key))
                throw new Exception("Jwt ayarlarındaki key boş olamaz.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAyarlari.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", id.ToString()),
                new Claim("roleId", roleId.ToString())
            };

            var expireDate = isRemember
                ? DateTime.Now.AddDays(30)
                : DateTime.Now.AddHours(12);

            var token = new JwtSecurityToken(
                issuer: _jwtAyarlari.Issuer,
                audience: _jwtAyarlari.Audience,
                claims: claims,
                expires: expireDate,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
