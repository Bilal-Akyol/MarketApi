using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Abstract
{
    public interface IAuthService
    {
        UserLoginResponse Login(UserLoginRequest request);
        UserRegisterResponse Register(UserRegisterRequest request);
    }
}
