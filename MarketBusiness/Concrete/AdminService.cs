using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketEntity.DTO;
using MarketEntity.Models;
using MarketEntity.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBusiness.Concrete
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;

        public AdminService(IUserRepository userRepository,
            ICategoriesRepository categoriesRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository)
        {
            _userRepository = userRepository;
            _categoryRepository = categoriesRepository;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            
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
                    response.Errors.Add("Doğrulama Hatası");
                    return response;
                }

                var user = _userRepository.Get(x => x.Email == request.Email);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Böyle bir kullanıcı bulunamadı.");
                    return response;
                }

                if (user.RoleId != 2)
                {
                    response.Code = "400";
                    response.Errors.Add("Sadece Admin girişi kabul edilir.");
                    return response;
                }

                if (!VerifyPassword(request.Password, user.Password))
                {
                    response.Code = "400";
                    response.Errors.Add("Şifre Yanlıştır");
                    return response;
                }
                response.Code = "200";
                response.Message = "Giriş Başarılı";
                response.UserId = user.Id;
                response.RoleId = user.RoleId;

                response.Email = user.Email ?? "";
                response.FirstName = user.FirstName ?? "";
                response.LastName = user.LastName ?? "";
                response.Phone = user.Phone ?? "";
                return response;
            }
            catch(Exception ex) 
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }


        public AddCategoryResponse AddCategory(AddCategoryRequest request)
        {
            var response = new AddCategoryResponse();
            try
            {
                var validator = new AddCategoryValidator();
                var validatorResult = validator.Validate(request);
                if (!validatorResult.IsValid) 
                {
                    foreach (var err in validatorResult.Errors)
                    {
                        response.Errors.Add(err.ErrorMessage);
                    }
                    response.Code = "400";
                    response.Errors.Add("Doğrulama Hatası");
                    return response;
                }
                var category = new Categories();
                category.CategoryName = request.CategoryName;
                category.Status = true;
                category.CreatedDate = DateTime.UtcNow.AddHours(3);
                _categoryRepository.Add(category);

                response.Code = "200";
                response.Message = "Kategori Başarıyla Eklendi";
                return response;
            }
            catch(Exception ex) 
            {
                response.Code = "400";
                response.Errors.Add($"{ex.Message}");
                return response;
            }
        }


        public ProductCreateResponse CreateProduct(ProductCreateRequest request)
        {
            var response = new ProductCreateResponse();
            try 
            {
                var validator = new ProductCreateValidator();
                var validatorResult = validator.Validate(request);

                if (!validatorResult.IsValid) 
                {
                    foreach (var err in validatorResult.Errors)
                    
                        response.Errors.Add(err.ErrorMessage);
                        response.Code = "400";
                        response.Errors.Add("Doğrulama Hatası");
                        return response;
                    
                }
                var product = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock,
                    CategoryId = request.CategoryId,
                    IsActive = true
                };

                var createdProduct = _productRepository.Add(product);
                for (int i = 0; i < request.Photos.Count; i++)
                {
                    var p = request.Photos[i];

                    var photoEntity = new ProductImage
                    {
                        ProductId = createdProduct.Id,
                        Base64 = p.Base64,
                        ContentType = p.ContentType,
                        SizeBytes = Base64SizeHelper.GetBytesFromBase64(p.Base64),
                        IsCover = (i == 0)
                    };

                    _productImageRepository.Add(photoEntity);
                }
                response.Code = "200";
                response.Message = "Ürün oluşturuldu";
                response.ProductId = createdProduct.Id;
                return response;
            }
            catch(Exception ex) 
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }

        }


        public ProductListResponse GetAll()
        {
            var response = new ProductListResponse();

            try
            {
                var products = _productRepository.GetList();

                foreach (var pr in products)
                {
                    // cover foto bul
                    var cover = _productImageRepository.GetList(x => x.ProductId == pr.Id && x.IsCover)
                        .FirstOrDefault();

                    response.Products.Add(new ProductListItem
                    {
                        ProductId = pr.Id,
                        Name = pr.Name ?? "",
                        Price = pr.Price,
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


        public ProductUpdateResponse UpdateProduct(ProductUpdateRequest request)
        {
            var response = new ProductUpdateResponse();
            try
            {
                var validator = new ProductUpdateValidator();
                var resultValidator = validator.Validate(request);
                if (!resultValidator.IsValid) 
                {
                    foreach (var err in resultValidator.Errors)
                        response.Errors.Add(err.ErrorMessage);
                    response.Code = "400";
                    response.Errors.Add("Doğrulama Hatası");
                    return response;
                    
                }
                var product = _productRepository.Get(x => x.Id == request.ProductId);
                if (product == null) 
                {
                    response.Code = "400";
                    response.Errors.Add("ürün bulunamadı.");
                    return response;
                }

                product.Name = request.Name;
                product.Description = request.Description;
                product.Price = request.Price;
                product.Stock = request.Stock;
                _productRepository.Update(product);


                if(request.Photos !=null && request.Photos.Count > 0) 
                {
                    var oldPhotos = _productImageRepository.GetList(x => x.ProductId == product.Id);
                    foreach (var op in oldPhotos)
                        _productImageRepository.Delete(op);

                    for(int i=0; i<request.Photos.Count;i++)
                    {
                        var p = request.Photos[i];
                        var photoEntity = new ProductImage
                        {
                            ProductId = product.Id,
                            Base64 = p.Base64,
                            ContentType = p.ContentType,
                            SizeBytes = Base64SizeHelper.GetBytesFromBase64(p.Base64),
                            IsCover = (i == 0)
                        };
                        _productImageRepository.Add(photoEntity);
                    }

                }
                response.Code = "200";
                response.Message = "Ürün Güncellendi";
                response.ProductId = product.Id;
                return response;

            }
            catch(Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }





        private bool VerifyPassword(string password, string passwordHash)
        {
            // passwordHash null ise direkt false dön (hata patlamasın)
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        
    }
}
