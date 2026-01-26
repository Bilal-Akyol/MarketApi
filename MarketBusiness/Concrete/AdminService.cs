using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketData.Concrete.Ef;
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
        private readonly ISliderRepository _sliderRepository;
        private readonly IAboutRepository _aboutRepository;
        private readonly IContactRepository _contactRepository;

        public AdminService(IUserRepository userRepository,
            ICategoriesRepository categoriesRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            ISliderRepository sliderRepository,
            IAboutRepository aboutRepository,
            IContactRepository contactRepository
            )
        {
            _userRepository = userRepository;
            _categoryRepository = categoriesRepository;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _sliderRepository = sliderRepository;
            _aboutRepository = aboutRepository;
            _contactRepository = contactRepository;
            
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



        public SliderCreateResponse SliderCreate(SliderCreateRequest request)
        {
            var response = new SliderCreateResponse();

            try
            {
                var validator = new SliderCreateValidator();
                var result = validator.Validate(request);

                if(!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Errors.Add("Doğrulama Hatası");
                    return response;

                }

                var slider = new Slider
                {
                    Title = request.Title,
                    RedirectUrl = request.RedirectUrl,
                    ImageBase64 = request.ImageBase64,
                    ImageContentType = request.ImageContentType,
                    ImageSizeBytes=Base64SizeHelper.GetBytesFromBase64(request.ImageBase64),
                    IsActive=request.IsActive
                };

                var created = _sliderRepository.Add(slider);

                response.Code = "200";
                response.Message = "Slider Eklendi";
                response.SliderId = created.Id;
                return response;

            }

            catch(Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

       





        public SliderUpdateResponse SliderUpdate(SliderUpdateRequest request)
        {
            var response = new SliderUpdateResponse();

            try
            {
                // Validation
                var validator = new SliderUpdateValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Errors.Add("Doğrulama hatası");
                    return response;
                }

                // Slider var mı
                var slider = _sliderRepository.Get(x => x.Id == request.SliderId);
                if (slider == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Slider bulunamadı.");
                    return response;
                }

                //Alanları güncelle
                slider.Title = request.Title;
                slider.RedirectUrl = request.RedirectUrl;
                slider.IsActive = request.IsActive;

                //Resim geldiyse güncelle gelmediyse eskisi kalsın
                if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                {
                    slider.ImageBase64 = request.ImageBase64!;
                    slider.ImageContentType = request.ImageContentType!;
                    slider.ImageSizeBytes = Base64SizeHelper.GetBytesFromBase64(request.ImageBase64!);
                }

                _sliderRepository.Update(slider);

                response.Code = "200";
                response.Message = "Slider güncellendi";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.InnerException?.Message ?? ex.Message);
                return response;
            }
        }


        public AboutCreateResponse AboutCreate(AboutCreateRequest request)
        {
            var response = new AboutCreateResponse();
            try 
            {
                var validator = new AboutCreateValidator();
                var result = validator.Validate(request);
                if (!result.IsValid) 
                {
                    foreach (var err in result.Errors)
                    
                        response.Errors.Add(err.ErrorMessage);
                        response.Code = "400";
                        response.Errors.Add("Doğrulama hatası");
                        return response;

                    
                }
                var about = new About
                {
                    Title = request.Title,
                    Content = request.Content,

                    ImageBase64 = request.ImageBase64,
                    ImageContentType = request.ImageContentType,
                    ImageSizeBytes = Base64SizeHelper.GetBytesFromBase64(request.ImageBase64),

                    IsActive = request.IsActive
                };
                about.CreatedDate = DateTime.Now;
                var created = _aboutRepository.Add(about);

                response.Code = "200";
                response.Message = "Hakkımızda eklendi";
                response.AboutId = created.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }



        public AboutUpdateResponse AboutUpdate(AboutUpdateRequest request)
        {
            var response = new AboutUpdateResponse();

            try
            {
                
                var validator = new AboutUpdateValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Errors.Add("Doğrulama hatası");
                    return response;
                }

                //Kayıt var mı
                var about = _aboutRepository.Get(x => x.Id == request.AboutId);
                if (about == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Hakkımızda bulunamadı.");
                    return response;
                }

                // Alanları güncelle
                about.Title = request.Title;
                about.Content = request.Content;
                about.IsActive = request.IsActive;

                //Resim geldiyse güncelle, gelmediyse eskisi kalsın
                if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                {
                    about.ImageBase64 = request.ImageBase64!;
                    about.ImageContentType = request.ImageContentType ?? "image/jpeg";
                    about.ImageSizeBytes = Base64SizeHelper.GetBytesFromBase64(request.ImageBase64!);
                }

                // BaseEntity
                about.ModifiedDate = DateTime.Now;

                _aboutRepository.Update(about);

                response.Code = "200";
                response.Message = "Hakkımızda güncellendi";
                response.AboutId = about.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }



        public ContactCreateResponse ContactCreate(ContactCreateRequest request) 
        {
            var response = new ContactCreateResponse();
            try 
            {
                var validator = new ContactCreateValidator();
                var result = validator.Validate(request);
                if (!result.IsValid) 
                {
                    foreach (var err in result.Errors)
                    
                        response.Errors.Add(err.ErrorMessage);
                        response.Code = "400";
                        response.Errors.Add("Doğrulama Hatası");
                        return response;
                        
                    
                }
                var contact = new Contact
                {
                    Title = request.Title,
                    Content = request.Content,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address=request.Address,
                    MapUrl = request.MapUrl,
                    IsActive = request.IsActive

                };
                contact.CreatedDate = DateTime.Now;
                var created = _contactRepository.Add(contact);
                response.Code = "200";
                response.Message = "İletişim bilgileri eklendi";
                response.ContactId = created.Id;
                return response;
                
            }
            catch(Exception err)
            {
                response.Code = "400";
                response.Errors.Add(err.Message);
                return response;
            }

        }



        public ContactUpdateResponse ContactUpdate(ContactUpdateRequest request)
        {
            var response = new ContactUpdateResponse();
            try
            {
                var valiidator = new ContactUpdateValidator();
                var result = valiidator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);
                    response.Code = "400";
                    response.Errors.Add("Doğrulama Hatası");
                    return response;
                }
                var contact = _contactRepository.Get(c => c.Id == request.ContactId);
                if (contact == null)
                {
                    response.Code = "400";
                    response.Errors.Add("İletişim Bilgileri Bulunamadı");
                    return response;
                }

                contact.Title = request.Title;
                contact.Content = request.Content;
                contact.Phone = request.Phone;
                contact.Email = request.Email;
                contact.Address = request.Address;
                contact.MapUrl = request.MapUrl;

                contact.ModifiedDate = DateTime.Now;
                _contactRepository.Update(contact);


                response.Code = "200";
                response.Message = "İletişim bilgileri güncellendi";
                response.ContactId = contact.Id;
                return response;

            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }




        public DeleteProductResponse DeleteProduct(DeleteProductRequest request)
        {
            var response = new DeleteProductResponse();
            try
            {
                var product = _productRepository.Get(x => x.Id == request.ProductId);
                if (product == null)
                {
                    response.Code = "200";
                    response.Message = "Ürün bulunamadı.";
                    return response;
                }

                // Ürüne ait resimleri sil
                var images = _productImageRepository.GetList(x => x.ProductId == product.Id);
                if (images != null && images.Any())
                {
                    foreach (var img in images)
                        _productImageRepository.Delete(img);
                }

                _productRepository.Delete(product);

                response.Code = "200";
                response.Message = "Ürün silme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public DeleteCategoryResponse DeleteCategory(DeleteCategoryRequest request)
        {
            var response = new DeleteCategoryResponse();
            try
            {
                var category = _categoryRepository.Get(x => x.Id == request.CategoryId);

                if (category == null)
                {
                    response.Code = "200";
                    response.Message = "Kategori bulunamadı.";
                    return response;
                }

                // Bu kategoriye bağlı ürün var mı?
                var anyProduct = _productRepository.Get(x => x.CategoryId == category.Id);
                if (anyProduct != null)
                {
                    response.Code = "400";
                    response.Message = "Bu kategoriye bağlı ürünler bulunduğundan silme başarısız. Kategoriyi pasife almayı deneyiniz.";
                    return response;
                }

                _categoryRepository.Delete(category);

                response.Code = "200";
                response.Message = "Kategori silme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public DeleteSliderResponse DeleteSlider(DeleteSliderRequest request)
        {
            var response = new DeleteSliderResponse();
            try
            {
                var slider = _sliderRepository.Get(x => x.Id == request.SliderId);
                if (slider == null)
                {
                    response.Code = "200";
                    response.Message = "Slider bulunamadı.";
                    return response;
                }

                _sliderRepository.Delete(slider);

                response.Code = "200";
                response.Message = "Slider silme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }






        private bool VerifyPassword(string password, string passwordHash)
        {
            // passwordHash null ise direkt false dön
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }



        
    }
}
