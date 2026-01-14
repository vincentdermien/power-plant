using Microsoft.AspNetCore.Mvc;
using PowerPlantCodingChallenge.BusinessContracts;
using PowerPlantCodingChallenge.DTO;

namespace PowerPlantCodingChallenge.Controllers
{
    /// <summary>
    /// Controller that exposes an endpoint to calculate a production plan for a given load and set of power plants.
    /// </summary>
    /// <remarks>
    /// Route: <c>api/productionplan</c>.
    /// Depends on an <c>IProductionPlanBusiness</c> implementation to perform the calculation
    /// and an <c>ILogger&lt;ProductionPlanController&gt;</c> for logging.
    /// </remarks>
    [ApiController]
    [Route("api/productionplan")]
    public class ProductionPlanController(IProductionPlanBusiness productionPlanBusiness, ILogger<ProductionPlanController> logger) : ControllerBase
    {
        private readonly IProductionPlanBusiness _productionPlanBusiness = productionPlanBusiness;
        private readonly ILogger<ProductionPlanController> _logger = logger;

        /// <summary>
        /// Calculates the production plan for the provided parameters.
        /// </summary>
        /// <param name="productionPlanParam">
        /// The input parameters required to compute the production plan. This includes the required load,
        /// the list of power plants and their characteristics, and the fuel information.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> that:
        /// - Returns <c>200 OK</c> with the calculated <c>ProductionPlanResult</c> on success.
        /// - Returns <c>400 Bad Request</c> when the request body is null or invalid.
        /// - Returns <c>500 Problem</c> when an unexpected exception occurs during calculation.
        /// </returns>
        /// <response code="200">Successful calculation returns the production plan result.</response>
        /// <response code="400">Missing or invalid production plan parameters.</response>
        /// <response code="500">An exception occurred during calculation.</response>
        /// <remarks>
        /// The method logs the start and completion of the operation and delegates the core
        /// calculation to <c>IProductionPlanBusiness.Calculate</c>. Existing inline validation is preserved.
        /// </remarks>
        [HttpPost]
        public IActionResult Calculate([FromBody] ProductionPlanParam productionPlanParam)
        {
            _logger.LogInformation("Calculating Production Plan");

            //Validate the request
            if (productionPlanParam == null)
                return BadRequest("Production plan parameters must be specified");

            try
            {
                var productionPlanResult = _productionPlanBusiness.Calculate(productionPlanParam);
                _logger.LogInformation("Done calculating Production Plan");
                return Ok(productionPlanResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when calculating Production Plan");
                return Problem(e.Message);
            }
        }
    }
}
