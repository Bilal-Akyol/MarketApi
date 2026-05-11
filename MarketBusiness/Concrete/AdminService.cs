using MarketBusiness.Abstract;
using MarketData.Abstract;
using MarketData.Concrete.Ef;
using MarketEntity.DTO;
using MarketEntity.Enum;
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
        private readonly ILogoRepository _logoRepository;
        private readonly IUserSessionRepository _userSessionRepository;

        public AdminService(IUserRepository userRepository,
            ICategoriesRepository categoriesRepository,
            IProductRepository productRepository,
            IProductImageRepository productImageRepository,
            ISliderRepository sliderRepository,
            IAboutRepository aboutRepository,
            IContactRepository contactRepository,
            ILogoRepository logoRepository,
            IUserSessionRepository userSessionRepository
            )
        {
            _userRepository = userRepository;
            _categoryRepository = categoriesRepository;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _sliderRepository = sliderRepository;
            _aboutRepository = aboutRepository;
            _contactRepository = contactRepository;
            _logoRepository = logoRepository;
            _userSessionRepository = userSessionRepository;
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

                var existingCategory = _categoryRepository.Get(x =>
                    x.Status == true &&
                    x.CategoryName.ToLower() == request.CategoryName.ToLower());

                if (existingCategory != null)
                {
                    response.Code = "400";
                    response.Message = "Bu kategori zaten mevcut.";
                    response.Errors.Add("Aynı isimde kategori eklenemez.");
                    return response;
                }

                var category = new Categories
                {
                    CategoryName = request.CategoryName.Trim(),
                    Status = true,
                    CreatedDate = DateTime.UtcNow.AddHours(3)
                };

                var addedCategory = _categoryRepository.Add(category);

                response.Code = "200";
                response.Message = "Kategori Başarıyla Eklendi";
                response.CategoryId = addedCategory.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
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




        public LogoCreateResponse CreateLogo(LogoCreateRequest request)
        {
            var response = new LogoCreateResponse();

            try
            {
                var validator = new LogoCreateValidator();
                var result = validator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                // Tek logo mantığı: varsa pasifleştir 
                var activeLogos = _logoRepository.GetList(x => x.IsActive == true && x.Status == true);
                foreach (var l in activeLogos)
                {
                    l.IsActive = false;
                    l.ModifiedDate = DateTime.Now;
                    _logoRepository.Update(l);
                }

                var logo = new Logo
                {
                    Title = request.Title,
                    ImageBase64 = request.ImageBase64,
                    ImageContentType = request.ImageContentType,
                    ImageSizeBytes = Base64SizeHelper.GetBytesFromBase64(request.ImageBase64),
                    IsActive = request.IsActive,
                    Status = true,
                    CreatedDate = DateTime.Now, 
                };

                var added = _logoRepository.Add(logo);

                response.LogoId = added.Id;
                response.Code = "200";
                response.Message = "Logo ekleme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public LogoUpdateResponse UpdateLogo(LogoUpdateRequest request)
        {
            var response = new LogoUpdateResponse();

            try
            {
                var validator = new LogoUpdateValidator();
                var result = validator.Validate(request);
                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var logo = _logoRepository.Get(x => x.Id == request.LogoId && x.Status == true);
                if (logo == null)
                {
                    response.Code = "400";
                    response.Message = "Logo bulunamadı.";
                    return response;
                }

                logo.Title = request.Title;
                logo.ImageBase64 = request.ImageBase64;
                logo.ImageContentType = request.ImageContentType;
                logo.IsActive = request.IsActive;
                logo.ModifiedDate = DateTime.Now;

                _logoRepository.Update(logo);

                response.LogoId = logo.Id;
                response.Code = "200";
                response.Message = "Logo güncelleme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public DeleteLogoResponse DeleteLogo(DeleteLogoRequest request)
        {
            var response = new DeleteLogoResponse();

            try
            {
                var logo = _logoRepository.Get(x => x.Id == request.LogoId && x.Status == true);
                if (logo == null)
                {
                    response.Code = "200";
                    response.Message = "Logo bulunamadı.";
                    return response;
                }

                _logoRepository.Delete(logo);

                response.Code = "200";
                response.Message = "Logo silme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }


        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest request)
        {
            var response = new UpdateCategoryResponse();

            try
            {
                var validator = new UpdateCategoryValidator();
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

                var category = _categoryRepository.Get(x => x.Id == request.CategoryId && x.Status == true);
                if (category == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kategori bulunamadı.");
                    return response;
                }

                var sameNameCategory = _categoryRepository.Get(x =>
                    x.Id != request.CategoryId &&
                    x.Status == true &&
                    x.CategoryName.ToLower() == request.CategoryName.ToLower());

                if (sameNameCategory != null)
                {
                    response.Code = "400";
                    response.Errors.Add("Aynı isimde başka bir kategori zaten mevcut.");
                    return response;
                }

                category.CategoryName = request.CategoryName;
                category.ModifiedDate = DateTime.UtcNow.AddHours(3);

                _categoryRepository.Update(category);

                response.Code = "200";
                response.Message = "Kategori başarıyla güncellendi";
                response.CategoryId = category.Id;
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public DeleteContactResponse DeleteContact(DeleteContactRequest request)
        {
            var response = new DeleteContactResponse();

            try
            {
                var contact = _contactRepository.Get(x => x.Id == request.ContactId && x.Status == true);

                if (contact == null)
                {
                    response.Code = "400";
                    response.Message = "İletişim bilgisi bulunamadı.";
                    return response;
                }

                _contactRepository.Delete(contact);

                response.Code = "200";
                response.Message = "İletişim bilgisi silme başarılı.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }
        public AdminGetAllUsersResponse AdminGetAllUsers(AdminGetAllUsersRequest request)
        {
            var response = new AdminGetAllUsersResponse();

            try
            {
                request ??= new AdminGetAllUsersRequest();

                var validator = new AdminGetAllUsersValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var query = _userRepository.GetList().AsQueryable();

                if (request.Status.HasValue)
                    query = query.Where(x => x.Status == request.Status.Value);

                var totalCount = query.Count();
                var page = request.Page;
                var pageSize = request.PageSize;

                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                if (totalPages == 0)
                    totalPages = 1;

                if (page > totalPages)
                    page = totalPages;

                var users = query
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var user in users)
                {
                    response.Users.Add(new AdminUserListModel
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.Phone,
                        RoleId = (long)user.RoleId,
                        EmailConfirmed = user.EmailConfirmed,
                        Status = user.Status,
                        CreatedDate = user.CreatedDate,
                        ModifiedDate = user.ModifiedDate,
                        DeletedDate = user.DeletedDate
                    });
                }

                response.Page = page;
                response.PageSize = pageSize;
                response.TotalCount = totalCount;
                response.TotalPages = totalPages;
                response.Code = "200";
                response.Message = "Kullanıcılar listelendi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public AdminGetUserByIdResponse AdminGetUserById(AdminGetUserByIdRequest request)
        {
            var response = new AdminGetUserByIdResponse();

            try
            {
                var validator = new AdminGetUserByIdValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.TargetUserId);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                response.User = new AdminUserDetailModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    RoleId = (long)user.RoleId,
                    EmailConfirmed = user.EmailConfirmed,
                    Status = user.Status,
                    Ip = user.Ip,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate,
                    DeletedDate = user.DeletedDate
                };

                response.Code = "200";
                response.Message = "Kullanıcı detayı getirildi.";
                return response;
            }
            catch (Exception ex)
            {
                response.Code = "400";
                response.Errors.Add(ex.Message);
                return response;
            }
        }

        public AdminDeleteUserResponse AdminDeleteUser(AdminDeleteUserRequest request)
        {
            var response = new AdminDeleteUserResponse();

            try
            {
                var validator = new AdminDeleteUserValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    foreach (var err in result.Errors)
                        response.Errors.Add(err.ErrorMessage);

                    response.Code = "400";
                    response.Message = "Doğrulama hatası";
                    return response;
                }

                var user = _userRepository.Get(x => x.Id == request.TargetUserId);
                if (user == null)
                {
                    response.Code = "400";
                    response.Errors.Add("Kullanıcı bulunamadı.");
                    return response;
                }

                if ((long)user.RoleId == (long)Role.Admin)
                {
                    response.Code = "400";
                    response.Errors.Add("Admin kullanıcı pasife çekilemez.");
                    return response;
                }

                if (!user.Status)
                {
                    response.Code = "200";
                    response.Message = "Kullanıcı zaten pasif.";
                    response.DeletedUserId = user.Id;
                    return response;
                }

                var now = DateTime.UtcNow;

                var sessions = _userSessionRepository.GetList(x =>
                    x.UserId == user.Id &&
                    x.Status == true &&
                    x.IsActive == true);

                foreach (var session in sessions)
                {
                    session.IsActive = false;
                    session.ExpireAt = now;
                    session.ModifiedDate = now;
                    _userSessionRepository.Update(session);
                }

                user.Status = false;
                user.DeletedDate = now;
                user.ModifiedDate = now;
                _userRepository.Update(user);

                response.DeletedUserId = user.Id;
                response.Code = "200";
                response.Message = "Kullanıcı pasife çekildi.";
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
