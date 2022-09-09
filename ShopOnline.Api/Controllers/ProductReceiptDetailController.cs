using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ShopOnline.Api.Controllers
{
    using Microsoft.EntityFrameworkCore;
    using ShopOnline.Api.Data;
    using ShopOnline.Api.Entities;
    using ShopOnline.Api.Extensions;
    using ShopOnline.Api.Repositories.Contracts;
    using ShopOnline.Models.Dtos;


    [Route("api/[controller]")]
    [ApiController]
    public class ProductReceiptDetailController : Controller
    {
        private readonly IProductReceiptDetailRepository productReceiptDetailRepository;
        private readonly IProductRepository productRepository;
        private readonly IProductReceiptRepository productReceiptRepository;
        private readonly ISupplierRepository supplierRepository;

        public ProductReceiptDetailController(IProductReceiptDetailRepository productReceiptDetailRepository,
                                      IProductRepository productRepository,
                                      IProductReceiptRepository productReceiptRepository,
                                      ISupplierRepository supplierRepository)
        {
            this.productReceiptDetailRepository = productReceiptDetailRepository;
            this.productRepository = productRepository;
            this.productReceiptRepository = productReceiptRepository;
            this.supplierRepository = supplierRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReceiptDetailDto>>> GetItems()
        {
            try
            {
                var productReceiptDetails = await this.productReceiptDetailRepository.GetItems();


                if (productReceiptDetails == null)
                { 
                   return NotFound();
                }
                else
                {
                    var products = await this.productRepository.GetItems();
                    var productReceipts = await this.productReceiptRepository.GetItems();
                    var suppliers = await this.supplierRepository.GetItems();
                    var productReceiptDetailDtos = productReceiptDetails.ConvertToDto(products, productReceipts,suppliers);
                    return Ok(productReceiptDetailDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
               
            }
        }
        //Items per Voucher ID
        [HttpGet("{dummy}/{voucherid}")]
        public async Task<ActionResult<IEnumerable<ProductReceiptDetailDto>>> GetItems(int voucherid)
        {
            try
            {
                var productReceiptDetails = await this.productReceiptDetailRepository.GetItemsByVoucherID(voucherid);
                if (productReceiptDetails == null)
                {
                    return NotFound();
                }
                else
                {
                    var products = await this.productRepository.GetItems();
                    var productReceipts = await this.productReceiptRepository.GetItems();
                    var suppliers = await this.supplierRepository.GetItems();
                    var productReceiptDetailDtos = productReceiptDetails.ConvertToDto(products, productReceipts, suppliers);
                    return Ok(productReceiptDetailDtos);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductReceiptDetailDto>> GetItem(int id)
        {
            try
            {
                var productReceiptDetail = await this.productReceiptDetailRepository.GetItem(id);

                if (productReceiptDetail == null)
                {
                    return BadRequest();
                }
                else
                {
                    var product= await this.productRepository.GetItem(productReceiptDetail.ProductId);

                    var productReceiptDetailDto = productReceiptDetail.ConvertToDto(product);

                    return Ok(productReceiptDetailDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpPatch()]
        public async Task<ActionResult<List<ProductReceiptDetailDto>>> UpdateItems(List<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            try
            {
                var productReceiptDetails = await this.productReceiptDetailRepository.UpdateItems(productReceiptDetailDtos);
                if (productReceiptDetails == null)
                {
                    return NotFound();
                }
                var products = await this.productRepository.GetItems();
                 return Ok(productReceiptDetails.ConvertToDto(products));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost]
        public async Task<ActionResult<ProductReceiptDetailDto>> PostItem([FromBody] ProductReceiptDetailDto productReceiptDetailDto)
        {
            try
            {
                var newProductReceiptDetail = await this.productReceiptDetailRepository.AddItem(productReceiptDetailDto);

                if (newProductReceiptDetail == null)
                {
                    return NoContent();
                }

                var product = await this.productRepository.GetItem(newProductReceiptDetail.ProductId);

                var newProductReceiptDetailDto = newProductReceiptDetail.ConvertToDto(product);

                return CreatedAtAction(nameof(GetItem), new { id = newProductReceiptDetailDto.Id }, newProductReceiptDetailDto);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("{dummy:int}")]
        public async Task<ActionResult<IEnumerable<ProductReceiptDetailDto>>> PostItems(int dummy,[FromBody] IEnumerable<ProductReceiptDetailDto> productReceiptDetailDtos)
        {
            try
            {
                var newProductReceiptDetails = await this.productReceiptDetailRepository.AddItems(productReceiptDetailDtos);

                if (newProductReceiptDetails == null)
                {
                    return NoContent();
                }

                var products = await this.productRepository.GetItems();
                var productReceipts = await this.productReceiptRepository.GetItems();
                var suppliers = await this.supplierRepository.GetItems();

                var newProductReceiptDetailDtos = newProductReceiptDetails.ConvertToDto(products, productReceipts, suppliers);

                return Ok(new List<ProductReceiptDetailDto>());


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            try
            {
                var productReceiptDetail = await this.productReceiptDetailRepository.DeleteItem(id);

                if (productReceiptDetail == null)
                {
                    return NotFound();
                }

               

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
