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
        
        ProductUpdateResponse UpdateProduct(ProductUpdateRequest request);
        SliderCreateResponse SliderCreate(SliderCreateRequest request);
        
        SliderUpdateResponse SliderUpdate(SliderUpdateRequest request);
        AboutCreateResponse AboutCreate(AboutCreateRequest request);
        AboutUpdateResponse AboutUpdate(AboutUpdateRequest request);
        



    }
}
