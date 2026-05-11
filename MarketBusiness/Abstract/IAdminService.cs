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


        AddCategoryResponse AddCategory(AddCategoryRequest request);


        ProductCreateResponse CreateProduct(ProductCreateRequest request);
        ProductUpdateResponse UpdateProduct(ProductUpdateRequest request);


        SliderCreateResponse SliderCreate(SliderCreateRequest request);
        SliderUpdateResponse SliderUpdate(SliderUpdateRequest request);


        AboutCreateResponse AboutCreate(AboutCreateRequest request);
        AboutUpdateResponse AboutUpdate(AboutUpdateRequest request);


        ContactCreateResponse ContactCreate(ContactCreateRequest request);
        ContactUpdateResponse ContactUpdate(ContactUpdateRequest request);


        DeleteProductResponse DeleteProduct(DeleteProductRequest request);
        DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest request);
        DeleteSliderResponse DeleteSlider(DeleteSliderRequest request);


        LogoCreateResponse CreateLogo(LogoCreateRequest request);
        LogoUpdateResponse UpdateLogo(LogoUpdateRequest request);
        DeleteLogoResponse DeleteLogo(DeleteLogoRequest request);
        UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request);
        DeleteContactResponse DeleteContact(DeleteContactRequest request);
        AdminGetAllUsersResponse AdminGetAllUsers(AdminGetAllUsersRequest request);
        AdminGetUserByIdResponse AdminGetUserById(AdminGetUserByIdRequest request);
        AdminDeleteUserResponse AdminDeleteUser(AdminDeleteUserRequest request);



    }
}
