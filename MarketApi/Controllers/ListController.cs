using MarketApi.Extensions;
using MarketBusiness.Abstract;
using MarketBusiness.Concrete;
using MarketEntity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace MarketApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        private readonly IListService _listService;


        public ListController(IListService listService, IOptions<Jwt> jwtAyarlari, ILogger<ListController> logger)
        {
            _listService = listService;

        }


        [SwaggerOperation(Summary = "Ürünleri listele (kapak foto ile)")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllProduct")]
        public ProductListResponse GetAll()
        {
            return _listService.GetAll();
        }


        [SwaggerOperation(Summary = "İd göre Ürün listele")]
        [AllowAnonymous]
        [HttpPost]
        [Route("GetByIdProduct")]
        public ProductGetByIdResponse GetByIdProduct(ProductGetByIdRequest request)
        {
            return _listService.GetByIdProduct(request);
        }



        [AllowAnonymous]
        [SwaggerOperation(Summary = "Aktif Sliderı Listeleme")]
        [HttpGet]
        [Route("GetActiveSliders")]
        public SliderListResponse GetAllActiveSlider()
        {
            return _listService.GetAllActiveSlider();
        }




        [SwaggerOperation(Summary = "Kaetegorileri listele")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllCategori")]
        public CategoryGetAllResponse GetAllCategory()
        {
            return _listService.GetAllCategory();
        }



        [SwaggerOperation(Summary ="İd göre kategoriyi listele")]
        [AllowAnonymous]
        [HttpPost]
        [Route("GetByIdCategor")]
        public CategoryGetByIdResponse GetByIdCategory(CategoryGetByIdRequest request) 
        {
            return _listService.GetByIdCategory(request);
        }
        


        [SwaggerOperation(Summary ="Hakkımzda listele")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllAbout")]
        public AboutListResponse GetAllAbouts()
        {
            return _listService.GetAllAbouts();
        }

        [SwaggerOperation(Summary = "İletişim Bilgilerini listele")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllContact")]
        public ContactListResponse GetAllContact()
        {
            return _listService.GetAllContact();
        }

        [SwaggerOperation(Summary ="Logoyu Listele")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetLogos")]
        public LogoListResponse GetLogos()
        {
            return _listService.GetLogos();
        }

    }
}
