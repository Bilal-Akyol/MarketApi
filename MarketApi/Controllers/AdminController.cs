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

        

        [SwaggerOperation(Summary = "Kategori Ekleme")]
        [HttpPost]
        [Route("AddCategory")]
        public AddCategoryResponse AddCategory(AddCategoryRequest request)
        {
            var response = new AddCategoryResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.AddCategory(request);
        }

        [SwaggerOperation("Ürün ekleme")]
        [HttpPost]
        [Route("AddProduct")]
        public ProductCreateResponse CreateProduct(ProductCreateRequest request)
        {
            var response = new ProductCreateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.CreateProduct(request);
        }

        [SwaggerOperation("Ürün güncelleme")]
        [HttpPut]
        [Route("ProductAddUpdate")]
        public ProductUpdateResponse UpdateProduct(ProductUpdateRequest request)
        {
            var response = new ProductUpdateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.UpdateProduct(request);
        }

        [SwaggerOperation(Summary = "Slider Ekleme")]
        [HttpPost]
        [Route("CreateSlider")]
        public SliderCreateResponse SliderCreate(SliderCreateRequest request)
        {
            var response = new SliderCreateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.SliderCreate(request);
        }

        [SwaggerOperation(Summary = "Slider Güncelleme")]
        [HttpPut]
        [Route("UpdateSlider")]
        public SliderUpdateResponse SliderUpdate(SliderUpdateRequest request)
        {
            var response = new SliderUpdateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            request.UserId = userId;
            return _adminService.SliderUpdate(request);
        }

        [SwaggerOperation(Summary = "Hakkımızda Ekleme")]
        [HttpPost]
        [Route("CreateAbout")]
        public AboutCreateResponse AboutCreate(AboutCreateRequest request)
        {
            var response = new AboutCreateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.AboutCreate(request);
        }

        [SwaggerOperation(Summary = "Hakkımızda Güncelleme")]
        [HttpPut]
        [Route("UpdateAbout")]
        public AboutUpdateResponse AboutUpdate(AboutUpdateRequest request)
        {
            var response = new AboutUpdateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.AboutUpdate(request);
        }

        [SwaggerOperation(Summary = "İletişim Ekleme")]
        [HttpPost]
        [Route("CreateContact")]
        public ContactCreateResponse ContactCreate(ContactCreateRequest request)
        {
            var response = new ContactCreateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.ContactCreate(request);
        }

        [SwaggerOperation(Summary = "İletişim Bilgilerini Güncelleme")]
        [HttpPut]
        [Route("ContactUpdate")]
        public ContactUpdateResponse ContactUpdate(ContactUpdateRequest request)
        {
            var response = new ContactUpdateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.ContactUpdate(request);
        }

        [SwaggerOperation(Summary = "Logo Ekleme")]
        [HttpPost]
        [Route("CreateLogo")]
        public LogoCreateResponse CreateLogo(LogoCreateRequest request)
        {
            var response = new LogoCreateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            request.UserId = userId;
            return _adminService.CreateLogo(request);
        }

        [SwaggerOperation(Summary = "Logo Güncelleme")]
        [HttpPut]
        [Route("UpdateLogo")]
        public LogoUpdateResponse UpdateLogo(LogoUpdateRequest request)
        {
            var response = new LogoUpdateResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            request.UserId = userId;
            return _adminService.UpdateLogo(request);
        }

        [SwaggerOperation(Summary = "Logo Silme")]
        [HttpDelete]
        [Route("DeleteLogo")]
        public DeleteLogoResponse DeleteLogo(long logoId)
        {
            var response = new DeleteLogoResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new DeleteLogoRequest
            {
                UserId = userId,
                LogoId = logoId
            };

            return _adminService.DeleteLogo(request);
        }

        [SwaggerOperation(Summary = "Ürün Silme")]
        [HttpDelete]
        [Route("DeleteProduct")]
        public DeleteProductResponse DeleteProduct(long productId)
        {
            var response = new DeleteProductResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new DeleteProductRequest
            {
                UserId = userId,
                ProductId = productId
            };

            return _adminService.DeleteProduct(request);
        }

        [SwaggerOperation(Summary = "Kategori Silme")]
        [HttpDelete]
        [Route("DeleteCategory")]
        public DeleteCategoryResponse DeleteCategory(long categoryId)
        {
            var response = new DeleteCategoryResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new DeleteCategoryRequest
            {
                UserId = userId,
                CategoryId = categoryId
            };

            return _adminService.DeleteCategory(request);
        }

        [SwaggerOperation(Summary = "Slider Silme")]
        [HttpDelete]
        [Route("DeleteSlider")]
        public DeleteSliderResponse DeleteSlider(long sliderId)
        {
            var response = new DeleteSliderResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new DeleteSliderRequest
            {
                UserId = userId,
                SliderId = sliderId
            };

            return _adminService.DeleteSlider(request);
        }

        [SwaggerOperation(Summary = "Kategori Güncelleme")]
        [HttpPut]
        [Route("UpdateCategory")]
        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            var response = new UpdateCategoryResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.UpdateCategory(request);
        }

        [SwaggerOperation(Summary = "İletişim Bilgisi Silme")]
        [HttpDelete]
        [Route("DeleteContact")]
        public DeleteContactResponse DeleteContact(long contactId)
        {
            var response = new DeleteContactResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (roleId != 2)
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new DeleteContactRequest
            {
                UserId = userId,
                ContactId = contactId
            };

            return _adminService.DeleteContact(request);
        }

        private bool TryGetUserInfo(out long userId, out long roleId)
        {
            userId = 0;
            roleId = 0;

            var userIdClaim = User.FindFirst("userId")?.Value;
            var roleIdClaim = User.FindFirst("roleId")?.Value;

            if (!long.TryParse(userIdClaim, out userId))
                return false;

            if (!long.TryParse(roleIdClaim, out roleId))
                return false;

            return true;
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