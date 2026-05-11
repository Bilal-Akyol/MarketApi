using MarketBusiness.Abstract;
using MarketEntity.DTO;
using MarketEntity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MarketApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [SwaggerOperation(Summary = "Kendi profilimi getir")]
        [HttpGet]
        [Route("GetMyProfile")]
        public GetMyProfileResponse GetMyProfile()
        {
            var request = new GetMyProfileRequest();

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.GetMyProfile(request);
        }

        [SwaggerOperation(Summary = "Profil güncelle")]
        [HttpPut]
        [Route("UpdateProfile")]
        public UpdateProfileResponse UpdateProfile(UpdateProfileRequest request)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.UpdateProfile(request);
        }

        [SwaggerOperation(Summary = "Şifre değiştir")]
        [HttpPut]
        [Route("ChangeMyPassword")]
        public ChangeMyPasswordResponse ChangeMyPassword(ChangeMyPasswordRequest request)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.ChangeMyPassword(request);
        }

        [SwaggerOperation(Summary = "Hesabımı sil")]
        [HttpDelete]
        [Route("DeleteMyAccount")]
        public DeleteMyAccountResponse DeleteMyAccount()
        {
            var request = new DeleteMyAccountRequest();

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.DeleteMyAccount(request);
        }
        [SwaggerOperation(Summary = "Açık oturumlarımı getir")]
        [HttpGet]
        [Route("GetMySessions")]
        public GetMySessionsResponse GetMySessions()
        {
            var request = new GetMySessionsRequest();

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.GetMySessions(request);
        }

        [SwaggerOperation(Summary = "Tek bir oturumu kapat")]
        [HttpPost]
        [Route("LogoutSession")]
        public LogoutSessionResponse LogoutSession(LogoutSessionRequest request)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!long.TryParse(userIdStr, out var userId))
                userId = 0;

            request.UserId = userId;
            return _userService.LogoutSession(request);
        }
    }
}
