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
        private readonly IAboutRepository _aboutRepository;
        private readonly IContactRepository _contactRepository;

        public ListService(IProductRepository productRepository,
            ISliderRepository sliderRepository,
            IProductImageRepository productImageRepository,
            ICategoriesRepository categoriesRepository,
            IAboutRepository aboutRepository,
            IContactRepository contactRepository)
        {
            _productRepository = productRepository;
            _sliderRepository = sliderRepository;
            _productImageRepository = productImageRepository;
            _categoriesRepository = categoriesRepository;
            _aboutRepository = aboutRepository;
            _contactRepository = contactRepository;
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
        public ProductGetByIdResponse GetByIdProduct(ProductGetByIdRequest request)
        {
            var response = new ProductGetByIdResponse();
            if (request.ProductId <= 0)
            {
                response.Code = "400";
                response.Errors.Add("ProductId Zorunludur");
                return response;
            }
            var product = _productRepository.Get(p => p.Id == request.ProductId);
            var cover = _productImageRepository.GetList(x => x.ProductId == product.Id && x.IsCover)
                        .FirstOrDefault();
            if (product == null)
            {
                response.Code = "400";
                response.Errors.Add("Product Bulunamadı");
                return response;
            }
            response.ProductId = product.Id;
            response.Name = product.Name;
            response.Price = product.Price;
            response.CoverBase64 = cover?.Base64;
            response.CoverContentType = cover?.ContentType;
            response.Code = "200";
            response.Message = "Product Getirildi";
            return response;

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
        public AboutListResponse GetAllAbouts() 
        {
            var response = new AboutListResponse();
            try 
            {
                var abouts = _aboutRepository.GetList();
                foreach (var about in abouts)
                {
                    response.Abouts.Add(new AboutListItem { 
                    
                        Title=about.Title,
                        Content=about.Content,
                        ImageBase64=about.ImageBase64,
                        ImageContentType=about.ImageContentType
                    });
                    
                }

                response.Code = "200";
                response.Message = "Hakkımızda Listelendi";
                return response;
            }
            catch(Exception ex) 
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


        


        public CategoryGetByIdResponse GetByIdCategory(CategoryGetByIdRequest request) 
        {
            var response = new CategoryGetByIdResponse();
            if (request.CategoryId <= 0) 
            {
                response.Code = "400";
                response.Errors.Add("CategoryId Zorunludur");
                return response;
            }
            var categories = _categoriesRepository.Get(c => c.Id == request.CategoryId);
            if(categories == null)
            {
                response.Code = "400";
                response.Errors.Add("Kategori Bulunamadı");
                return response;
            }

            response.CategoryId = categories.Id;
            response.CategoryName = categories.CategoryName;
            response.Code = "200";
            response.Message = "Kategori getirildi";
            return response;
        }

        public ContactListResponse GetAllContact()
        {
            var response = new ContactListResponse();
            try
            {
                var contacts = _contactRepository.GetList();
                foreach (var contact in contacts)
                {
                    response.Contacts.Add(new ContactListItem
                    {

                        Title = contact.Title,
                        Content = contact.Content,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        Address = contact.Address,
                        MapUrl = contact.MapUrl

                    });
                }
                response.Code = "200";
                response.Message = "İletişim Bilgileri Listelendi";
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
