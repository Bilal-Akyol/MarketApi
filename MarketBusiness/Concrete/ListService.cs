using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketData.Concrete.Ef;
using MarketEntity.DTO;
using MarketEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Concrete
{
    public class ListService:IListService
    {

        private readonly IProductRepository _productRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public ListService(IProductRepository productRepository,
            ISliderRepository sliderRepository,
            IProductImageRepository productImageRepository,
            ICategoriesRepository categoriesRepository)
        {
            _productRepository = productRepository;
            _sliderRepository = sliderRepository;
            _productImageRepository = productImageRepository;
            _categoriesRepository = categoriesRepository;
        }




        public ProductListResponse GetAll()
        {
            var response = new ProductListResponse();

            try
            {
                var products = _productRepository.GetList();

                foreach (var product in products)
                {
                    // cover foto bul
                    var cover = _productImageRepository.GetList(x => x.ProductId == product.Id && x.IsCover)
                        .FirstOrDefault();

                    response.Products.Add(new ProductListItem
                    {
                        ProductId = product.Id,
                        Name = product.Name ?? "",
                        Price = product.Price,
                        CoverBase64 = cover?.Base64,
                        CoverContentType = cover?.ContentType
                    });
                }

                response.Code = "200";
                response.Message = "Ürünler listelendi";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

       


        public SliderListResponse GetAllActiveSlider()
        {
            var response = new SliderListResponse();
            try
            {
                var sliders = _sliderRepository.GetList(x => x.IsActive)
                    .ToList();
                foreach (var s in sliders)
                {
                    response.Sliders.Add(new SliderListItem
                    {
                        SliderId = s.Id,
                        Title = s.Title ?? "",
                        RedirectUrl = s.RedirectUrl,
                        ImageBase64 = s.ImageBase64,
                        ImageContentType = s.ImageContentType

                    });
                }

                response.Code = "200";
                response.Message = "Sliderlar Listelendi";
                return response;

            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }

        }

        public CategoryGetAllResponse GetAllCategory()
        {
            var response = new CategoryGetAllResponse();
            try
            {
                var categories = _categoriesRepository.GetList();
                foreach (var category in categories)
                {
                    response.Categories.Add(new CategoryListItem
                    {

                        CategoryId = category.Id,
                        CategoryName = category.CategoryName

                    });
                }
                response.Code = "200";
                response.Message = "Kategori Listelendi";
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
