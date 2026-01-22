using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Abstract
{
    public interface IListService
    {
        ProductListResponse GetAll();
        ProductGetByIdResponse GetByIdProduct(ProductGetByIdRequest request);
        SliderListResponse GetAllActiveSlider();
        CategoryGetAllResponse GetAllCategory();
        CategoryGetByIdResponse GetByIdCategory(CategoryGetByIdRequest request);
        AboutListResponse GetAllAbouts();
        ContactListResponse GetAllContact();
















        
    }
}
