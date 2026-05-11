using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Abstract
{
    public interface IUserService
    {
        GetMyProfileResponse GetMyProfile(GetMyProfileRequest request);
        UpdateProfileResponse UpdateProfile(UpdateProfileRequest request);
        ChangeMyPasswordResponse ChangeMyPassword(ChangeMyPasswordRequest request);
        DeleteMyAccountResponse DeleteMyAccount(DeleteMyAccountRequest request);
    }
}
