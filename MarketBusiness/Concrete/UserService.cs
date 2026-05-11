using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketEntity.DTO;
using MarketEntity.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSessionRepository _userSessionRepository;

        public UserService(
            IUserRepository userRepository,
            IUserSessionRepository userSessionRepository)
        {
            _userRepository = userRepository;
            _userSessionRepository = userSessionRepository;
        }

        public GetMyProfileResponse GetMyProfile(GetMyProfileRequest request)
        {
            var response = new GetMyProfileResponse();

            try
            {
                var validator = new GetMyProfileValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.UserId && x.Status == true);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                response.User = new UserProfileModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    RoleId = (long)user.RoleId,
                    EmailConfirmed = user.EmailConfirmed
                };

                response.Code = "200";
                response.Message = "Profil bilgileri getirildi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public UpdateProfileResponse UpdateProfile(UpdateProfileRequest request)
        {
            var response = new UpdateProfileResponse();

            try
            {
                var validator = new UpdateProfileValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.UserId && x.Status == true);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                var emailConflict = _userRepository.Get(x =>
                    x.Email == request.Email &&
                    x.Id != request.UserId &&
                    x.Status == true);

                if (emailConflict != null)
                {
                    response.Code = "400";
                    response.Errors.Add("Bu email başka bir kullanıcı tarafından kullanılıyor.");
                    return response;
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Phone = request.Phone;
                user.ModifiedDate = DateTime.UtcNow;

                _userRepository.Update(user);

                response.UserId = user.Id;
                response.Code = "200";
                response.Message = "Profil güncellendi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public ChangeMyPasswordResponse ChangeMyPassword(ChangeMyPasswordRequest request)
        {
            var response = new ChangeMyPasswordResponse();

            try
            {
                var validator = new ChangeMyPasswordValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.UserId && x.Status == true);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password))
                {
                    response.Code = "400";
                    response.Errors.Add("Mevcut şifre yanlış.");
                    return response;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.ModifiedDate = DateTime.UtcNow;
                _userRepository.Update(user);

                response.Code = "200";
                response.Message = "Şifre güncellendi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public DeleteMyAccountResponse DeleteMyAccount(DeleteMyAccountRequest request)
        {
            var response = new DeleteMyAccountResponse();

            try
            {
                var validator = new DeleteMyAccountValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.UserId && x.Status == true);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                var now = DateTime.UtcNow;

                var sessions = _userSessionRepository.GetList(x =>
                    x.UserId == request.UserId &&
                    x.Status == true &&
                    x.IsActive == true);

                foreach (var session in sessions)
                {
                    session.IsActive = false;
                    session.ExpireAt = now;
                    session.ModifiedDate = now;
                    _userSessionRepository.Update(session);
                }

                user.Status = false;
                user.DeletedDate = now;
                user.ModifiedDate = now;
                _userRepository.Update(user);

                response.UserId = user.Id;
                response.Code = "200";
                response.Message = "Hesabınız silinmiştir.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public GetMySessionsResponse GetMySessions(GetMySessionsRequest request)
        {
            var response = new GetMySessionsResponse();

            try
            {
                var validator = new GetMySessionsValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var sessions = _userSessionRepository
                    .GetList(x => x.UserId == request.UserId && x.Status == true)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();

                foreach (var session in sessions)
                {
                    response.Sessions.Add(new UserSessionListModel
                    {
                        SessionId = session.Id,
                        SessionToken = session.SessionToken,
                        IpAddress = session.IpAddress,
                        UserAgent = session.UserAgent,
                        ExpireAt = session.ExpireAt,
                        IsActive = session.IsActive,
                        CreatedDate = session.CreatedDate,
                        ModifiedDate = session.ModifiedDate
                    });
                }

                response.Code = "200";
                response.Message = "Oturumlar getirildi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public LogoutSessionResponse LogoutSession(LogoutSessionRequest request)
        {
            var response = new LogoutSessionResponse();

            try
            {
                var validator = new LogoutSessionValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var session = _userSessionRepository.Get(x =>
                    x.Id == request.SessionId &&
                    x.UserId == request.UserId &&
                    x.Status == true);

                if (session == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Oturum bulunamadı.");
                    return response;
                }

                if (!session.IsActive)
                {
                    response.Code = "200";
                    response.Message = "Oturum zaten kapalı.";
                    response.SessionId = session.Id;
                    return response;
                }

                session.IsActive = false;
                session.ExpireAt = DateTime.UtcNow;
                session.ModifiedDate = DateTime.UtcNow;
                _userSessionRepository.Update(session);

                response.SessionId = session.Id;
                response.Code = "200";
                response.Message = "Oturum kapatıldı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
    }
}
