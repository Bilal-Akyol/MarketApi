using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Abstract
{
    public interface IAdminService
    {
        UserLoginResponse Login(UserLoginRequest request);
        AddCategoryResponse AddCategory(AddCategoryRequest request);
        ProductCreateResponse CreateProduct(ProductCreateRequest request);
        ProductListResponse GetAll();
        ProductUpdateResponse UpdateProduct(ProductUpdateRequest request);
        



    }
}
