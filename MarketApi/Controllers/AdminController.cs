using MarketApi.Extensions;
using MarketBusiness.Abstract;
using MarketEntity.DTO;
using MarketEntity.Enum;
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

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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

            if (!IsAdmin(roleId))
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.AddCategory(request);
        }

        [SwaggerOperation(Summary = "Ürün ekleme")]
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

            if (!IsAdmin(roleId))
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok");
                return response;
            }

            request.UserId = userId;
            return _adminService.CreateProduct(request);
        }

        [SwaggerOperation(Summary = "Ürün güncelleme")]
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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

            if (!IsAdmin(roleId))
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
        [SwaggerOperation(Summary = "Kullanıcıları listele")]
        [HttpPost]
        [Route("GetAllUsers")]
        public AdminGetAllUsersResponse AdminGetAllUsers(AdminGetAllUsersRequest request)
        {
            var response = new AdminGetAllUsersResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (!IsAdmin(roleId))
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            request.UserId = userId;
            return _adminService.AdminGetAllUsers(request);
        }

        [SwaggerOperation(Summary = "Kullanıcı detay getir")]
        [HttpPost]
        [Route("GetUserById")]
        public AdminGetUserByIdResponse AdminGetUserById(AdminGetUserByIdRequest request)
        {
            var response = new AdminGetUserByIdResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (!IsAdmin(roleId))
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            request.UserId = userId;
            return _adminService.AdminGetUserById(request);
        }

        [SwaggerOperation(Summary = "Kullanıcı pasife çek")]
        [HttpDelete]
        [Route("DeleteUser")]
        public AdminDeleteUserResponse AdminDeleteUser(long targetUserId)
        {
            var response = new AdminDeleteUserResponse();

            if (!TryGetUserInfo(out var userId, out var roleId))
            {
                response.Code = "401";
                response.Message = "Yetkisiz erişim";
                response.Errors.Add("Token claim bilgileri okunamadı.");
                return response;
            }

            if (!IsAdmin(roleId))
            {
                response.Code = "400";
                response.Errors.Add("Bu işlemi yapmaya yetkiniz yok.");
                return response;
            }

            var request = new AdminDeleteUserRequest
            {
                UserId = userId,
                TargetUserId = targetUserId
            };

            return _adminService.AdminDeleteUser(request);
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

        private bool IsAdmin(long roleId)
        {
            return roleId == (long)Role.Admin;
        }
    }
}