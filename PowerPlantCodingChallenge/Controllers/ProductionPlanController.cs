using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PowerPlantCodingChallenge.Business;
using PowerPlantCodingChallenge.DTO;

namespace PowerPlantCodingChallenge.Controllers
{
    [ApiController]
    [Route("api/productionplan")]
    public class ProductionPlanController : ControllerBase
    {
        private readonly ProductionPlanBusiness _productionPlanBusiness = new();

        [HttpPost]
        public IActionResult Calculate([FromBody] ProductionPlanParam productionPlanParam)
        {
            //Validate the request
            if (productionPlanParam == null)
                return BadRequest("Production plan parameters must be specified");

            try
            {
                var productionPlanResult = _productionPlanBusiness.Calculate(productionPlanParam);

                return Ok(productionPlanResult);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }
}
