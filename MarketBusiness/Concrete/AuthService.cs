using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketData.Concrete.Ef;
using MarketEntity.DTO;
using MarketEntity.Enum;
using MarketEntity.Models;
using MarketEntity.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSessionRepository _userSessionRepository;

        public AuthService(IUserRepository userRepository, IUserSessionRepository userSessionRepository)
        {
            _userRepository = userRepository;
            _userSessionRepository = userSessionRepository;
        }

        public UserLoginResponse Login(UserLoginRequest request)
        {
            var response = new UserLoginResponse();

            try
            {
                var validator = new UserLoginValidator();
                var validatorResult = validator.Validate(request);

                if (!validatorResult.IsValid)
                {
                    foreach (var err in validatorResult.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama Hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Email == request.Email && x.Status == true);
                if (user == null)
                {
                    response.Code = "400";
                    response.Message = "Böyle bir kullanıcı bulunamadı.";
                    return response;
                }

                if (!user.EmailConfirmed)
                {
                    response.Code = "400";
                    response.Message = "Email doğrulaması tamamlanmamış.";
                    response.Errors.Add("Lütfen önce email doğrulamasını tamamlayın.");
                    return response;
                }

                if (!VerifyPassword(request.Password, user.Password))
                {
                    response.Code = "400";
                    response.Message = "Şifre yanlıştır.";
                    return response;
                }

                response.Code = "200";
                response.Message = "Giriş başarılı";
                response.UserId = user.Id;
                response.RoleId = (long)user.RoleId;
                response.Email = user.Email ?? "";
                response.FirstName = user.FirstName ?? "";
                response.LastName = user.LastName ?? "";
                response.Phone = user.Phone ?? "";

                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public UserRegisterResponse Register(UserRegisterRequest request)
        {
            var response = new UserRegisterResponse();

            try
            {
                var validator = new UserRegisterValidator();
                var validatorResult = validator.Validate(request);

                if (!validatorResult.IsValid)
                {
                    foreach (var err in validatorResult.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama Hatası";
                    return response;
                }

                var exists = _userRepository.Get(x => x.Email == request.Email && x.Status == true);
                if (exists != null)
                {
                    response.Code = "400";
                    response.Message = "Bu email ile kayıtlı kullanıcı zaten var.";
                    return response;
                }

                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Phone = request.Phone,
                    Ip = request.Ip,
                    EmailConfirmed = true,
                    RoleId = Role.User,
                    Status = true,
                    CreatedDate = DateTime.UtcNow
                };

                var createdUser = _userRepository.Add(user);

                response.Code = "200";
                response.Message = "Kayıt başarılı";
                response.UserId = createdUser.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public LogoutResponse Logout(LogoutRequest request)
        {
            var response = new LogoutResponse();

            try
            {
                if (request.UserId <= 0)
                {
                    response.Code = "400";
                    response.Errors.Add("UserId zorunludur.");
                    return response;
                }

                var now = DateTime.UtcNow;

                var sessions = _userSessionRepository.GetList(x =>
                    x.UserId == request.UserId &&
                    x.IsActive == true &&
                    x.Status == true);

                foreach (var session in sessions)
                {
                    session.IsActive = false;
                    session.ExpireAt = now;
                    session.ModifiedDate = now;
                    _userSessionRepository.Update(session);
                }

                response.Code = "200";
                response.Message = "Çıkış yapıldı. Tüm oturumlar kapatıldı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }


        private bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}