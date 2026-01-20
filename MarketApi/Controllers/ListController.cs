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

    }
}
