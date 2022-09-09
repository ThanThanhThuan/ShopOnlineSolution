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
    public class ProductReceiptController : Controller
    {

        private readonly IProductReceiptRepository productReceiptRepository;
        private readonly ISupplierRepository supplierRepository;

        public ProductReceiptController(IProductReceiptRepository productReceiptRepository, ISupplierRepository supplierRepository)
        {
            this.productReceiptRepository = productReceiptRepository;
            this.supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReceiptDto>>> GetItems()
        {
            try
            {
                var productReceipts = await this.productReceiptRepository.GetItems();

                if (productReceipts == null)
                {
                    return NoContent();
                }


                var productReceiptDtos = productReceipts.ConvertToDto();

                return Ok(productReceiptDtos);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductReceiptDto>> GetItem(int id)
        {
            try
            {
                var productReceipt = await this.productReceiptRepository.GetItem(id);

                if (productReceipt == null)
                {
                    return BadRequest();
                }
                else
                {
                    var suppliers= await this.supplierRepository.GetItems();
                    var productReceiptDto = productReceipt.ConvertToDto(suppliers);

                    return Ok(productReceiptDto);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
        [HttpDelete()]
        public async Task<ActionResult<int>> DeleteItems([FromBody] List<int> ids)
        {
            var i = await this.productReceiptRepository.DeleteItems(ids);
            return Ok(i);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductReceiptDto>> DeleteItem(int id)
        {
            try
            {
                var productReceipt = await this.productReceiptRepository.DeleteItem(id);

                if (productReceipt == null)
                {
                    return NotFound();
                }
                var suppliers = await this.supplierRepository.GetItems();

                var productReceiptDto = productReceipt.ConvertToDto(suppliers);

                return Ok(productReceiptDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ProductReceiptDto>> UpdateItem(int id, ProductReceiptDto productReceiptDto)
        {
            try
            {
                var productReceipt = await this.productReceiptRepository.UpdateItem(id, productReceiptDto);
                if (productReceipt == null)
                {
                    return NotFound();
                }
                var suppliers = await this.supplierRepository.GetItems();
                var resproductReceiptDto = productReceipt.ConvertToDto(suppliers);

                return Ok(resproductReceiptDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
       
        [HttpPost]
        public async Task<ActionResult<ProductReceiptDto>> PostItem([FromBody] ProductReceiptDto productReceiptDto)
        {
            try
            {
                var newProductReceipt = await this.productReceiptRepository.AddItem(productReceiptDto);

                if (newProductReceipt == null)
                {
                    return NoContent();
                }

                var suppliers = await this.supplierRepository.GetItems();
                var newProductReceiptDto = newProductReceipt.ConvertToDto(suppliers);

                return CreatedAtAction(nameof(GetItem), new { id = newProductReceiptDto.Id }, newProductReceiptDto);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
