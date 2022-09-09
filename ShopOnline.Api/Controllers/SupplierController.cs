using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository supplierRepository;
        public SupplierController(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;   
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetItems()
        {
            try
            {
                var suppliers = await this.supplierRepository.GetItems();


                if (suppliers == null)
                {
                    return NotFound();
                }
                else
                {
           
                    return Ok(suppliers.ConvertToDto());
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");

            }
        }
    }
}
