using MarketApi.Extensions;
using MarketBusiness.Abstract;
using MarketEntity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace MarketApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly Jwt _jwtAyarlari;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, IOptions<Jwt> jwtAyarlari, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _jwtAyarlari = jwtAyarlari.Value;
            _logger = logger;

        }

        [SwaggerOperation(Summary = "Admin Girişi Yap")]
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public UserLoginResponse Login(UserLoginRequest request)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation("Login post method Starting. -" + "Ip adresi;" + ip + "Email:" + request.Email);
            UserLoginResponse userLoginResponse = _adminService.Login(request);


            if (userLoginResponse.Code.Equals("200"))
            {
                userLoginResponse.Token = GetToken(request.isRemember, userLoginResponse.UserId, userLoginResponse.RoleId);
            }

            return userLoginResponse;
        }

        [SwaggerOperation(Summary ="Kategori Ekleme")]
        [HttpPost]
        [Route("AddCategory")]
        public AddCategoryResponse AddCategory(AddCategoryRequest request) 
        {
            var identity = User.Identity as ClaimsIdentity;
            if(identity != null) 
            {
                var id = Convert.ToInt64(identity.Claims.ElementAt(0).Value);
                request.UserId = id;


                var roleId = Convert.ToInt64(identity.Claims.ElementAt(1).Value);

                if(roleId !=2)
                {
                    AddCategoryResponse response = new AddCategoryResponse();
                    response.Code = "400";
                    response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                    return response;
                }
            }
            return _adminService.AddCategory(request);
        }

        [SwaggerOperation("Ürün ekleme")]
        [HttpPost]
        [Route("AddProduct")]
        public ProductCreateResponse CreateProduct(ProductCreateRequest request) 
        {
            
            var identity = User.Identity as ClaimsIdentity;

            if (identity != null) 
            {
                var userId = Convert.ToInt64(identity.Claims.ElementAt(0).Value);
                request.UserId = userId;

                // 2) Token içindeki RoleId'yi al
                var roleId = Convert.ToInt64(identity.Claims.ElementAt(1).Value);

                if (roleId != 2) 
                {
                    ProductCreateResponse response = new ProductCreateResponse();
                    response.Code = "400";
                    response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                    return response;
                }
            }
            return _adminService.CreateProduct(request);

        }


        
        [SwaggerOperation(Summary = "Ürünleri listele (kapak foto ile)")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public ProductListResponse GetAll()
        {
            return _adminService.GetAll();
        }

        [SwaggerOperation("Ürün güncelleme")]
        [HttpPost]
        [Route("ProductAddUpdate")]
        public ProductUpdateResponse UpdateProduct(ProductUpdateRequest request)
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userId = Convert.ToInt64(identity.Claims.ElementAt(0).Value);
                request.UserId = userId;

                // Token içindeki RoleId
                var roleId = Convert.ToInt64(identity.Claims.ElementAt(1).Value);

                if (roleId != 2)
                {
                    ProductUpdateResponse response = new ProductUpdateResponse();
                    response.Code = "400";
                    response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                    return response;
                }
            }

            return _adminService.UpdateProduct(request);
        }

        [SwaggerOperation(Summary ="Slider Ekleme")]
        [HttpPost]
        [Route("CreateSlider")]
        public SliderCreateResponse SliderCreate(SliderCreateRequest request) 
        {
            var identity = User.Identity as ClaimsIdentity;
            if(identity != null) 
            {
                var userId = Convert.ToInt64(identity.Claims.ElementAt(0).Value);

                var roleId = Convert.ToInt64(identity.Claims.ElementAt(1).Value);
                if(roleId != 2) 
                {
                    var response = new SliderCreateResponse();
                    response.Code = "400";
                    response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                    return response;
                }
            }

            return _adminService.SliderCreate(request);

        }

        [SwaggerOperation(Summary ="Slider Güncelleme")]
        [HttpPost]
        [Route("UpdateSlider")]
        public SliderUpdateResponse SliderUpdate(SliderUpdateRequest request) 
        {
            var identity = User.Identity as ClaimsIdentity;

            if(identity !=null)
            {
                var userId = Convert.ToInt64(identity.Claims.ElementAt(0).Value);
                request.UserId = userId;

                var roleId = Convert.ToInt64(identity.Claims.ElementAt(1).Value);

                if (roleId != 2) 
                {
                    var response = new SliderUpdateResponse();
                    response.Code = "400";
                    response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                    return response;
                }

            }
            return _adminService.SliderUpdate(request);
        }


        [AllowAnonymous]
        [SwaggerOperation(Summary ="Aktif Sliderı Listeleme")]
        [HttpGet]
        [Route("GetActiveSliders")]
        public SliderListResponse GetAllActiveSlider()
        {
            return _adminService.GetAllActiveSlider();
        }





        private string GetToken(bool isRemember, long id, long roleId)
        {
            if (_jwtAyarlari.Key == null) throw new Exception("Jwt ayarlarındaki key boş olmaz.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAyarlari.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("value",id.ToString()),
                new Claim("value",roleId.ToString())
            };

            var token = new JwtSecurityToken(_jwtAyarlari.Issuer,
                _jwtAyarlari.Audience,
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
