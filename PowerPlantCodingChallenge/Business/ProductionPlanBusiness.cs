using PowerPlantCodingChallenge.BusinessContracts;
using PowerPlantCodingChallenge.DTO;
using PowerPlantCodingChallenge.Utils;

namespace PowerPlantCodingChallenge.Business
{
    /// <summary>
    /// Service that computes a production plan to satisfy a requested load using the available power plants.
    /// </summary>
    /// <remarks>
    /// The plan is computed in two phases:
    /// 1. Initialize each plant at its maximum available power (wind-adjusted for wind turbines).
    /// 2. If the total available power exceeds the requested load, reduce output starting from the most expensive
    ///    plants first while respecting each plant's minimum operating power (<c>PMin</c>).
    ///
    /// Costs for fuel-based plants are calculated as fuel price per MWh divided by plant efficiency.
    ///
    /// Complexity: O(n log n) dominated by sorting plants by cost when reducing excess power.
    /// </remarks>
    /// <param name="logger">Logger used for error reporting and diagnostics.</param>
    public class ProductionPlanBusiness(ILogger<ProductionPlanBusiness> logger) : IProductionPlanBusiness
    {
        private readonly ILogger<ProductionPlanBusiness> _logger = logger;

        /// <summary>
        /// Calculates the production plan for the given parameters.
        /// </summary>
        /// <param name="productionPlanParams">
        /// Input parameters including:
        /// - <see cref="ProductionPlanParam.Load"/>: required load to satisfy,
        /// - <see cref="ProductionPlanParam.Fuels"/>: fuel prices and wind percentage,
        /// - <see cref="ProductionPlanParam.PowerPlants"/>: list of available power plants.
        /// </param>
        /// <returns>
        /// A list of <see cref="ProductionPlanResult"/> entries. Each entry contains:
        /// - <see cref="ProductionPlanResult.PowerPlantName"/>,
        /// - allocated <see cref="ProductionPlanResult.Power"/> output,
        /// - <see cref="ProductionPlanResult.PMin"/> for the plant,
        /// - computed <see cref="ProductionPlanResult.CostEuroPerMWh"/> used to prioritize reductions.
        /// </returns>
        /// <exception cref="System.Exception">
        /// Thrown when the combined available power of all plants is less than the requested load.
        /// </exception>
        public List<ProductionPlanResult> Calculate(ProductionPlanParam productionPlanParams)
        {
            var productionPlanResultList = new List<ProductionPlanResult>();

            //Init the result with max power in all power plants
            foreach (var powerPlant in productionPlanParams.PowerPlants)
            {
                productionPlanResultList.Add(new ProductionPlanResult()
                {
                    PowerPlantName = powerPlant.Name,
                    Power = CalculatePowerPlantAvailablePower(powerPlant, productionPlanParams.Fuels.WindPercentage),
                    PMin = powerPlant.PMin,
                    CostEuroPerMWh = CalculatePowerPlantCost(powerPlant, productionPlanParams)
                });
            }

            //Compute available power
            var availablePower = productionPlanResultList.Sum(x => x.Power);

            //Maybe we can't satisfy the demand
            if (availablePower < productionPlanParams.Load)
            {
                _logger.LogError("It was not possible to meet the required load with the available power plants.");
                throw new Exception("It was not possible to meet the required load with the available power plants.");
            }

            //Compute power excess
            var powerExcess = availablePower - productionPlanParams.Load;

            if (powerExcess > decimal.Zero)
            {
                //Reduce power in most expensive plants first
                foreach (var powerPlant in productionPlanResultList.OrderByDescending(x => x.CostEuroPerMWh))
                {
                    //Can we switch off this plant completely?
                    if (powerPlant.Power <= powerExcess)
                    {
                        powerExcess -= powerPlant.Power;
                        powerPlant.Power = decimal.Zero;
                        continue;
                    }

                    //Let the power plant on but reduce the power, max until PMin
                    var reduciblePower = powerPlant.Power - powerPlant.PMin;
                    if (reduciblePower <= powerExcess)
                    {
                        powerPlant.Power -= reduciblePower;
                        powerExcess -= reduciblePower;
                    }
                    else
                    {
                        powerPlant.Power -= powerExcess;

                        //Exit the loop, we reached what we wanted
                        break;
                    }
                }
            }

            return productionPlanResultList;
        }

        /// <summary>
        /// Computes the maximum available power for a single power plant.
        /// </summary>
        /// <param name="powerPlant">Power plant definition that includes <see cref="PowerPlantDto.PMax"/> and <see cref="PowerPlantDto.Type"/>.</param>
        /// <param name="windPercentage">Wind availability expressed as a percentage (0-100). Used only for wind turbines.</param>
        /// <returns>
        /// The available power in MW. For wind turbines the value is wind-adjusted and rounded to one decimal place;
        /// for other plant types the <see cref="PowerPlantDto.PMax"/> is returned.
        /// </returns>
        private static decimal CalculatePowerPlantAvailablePower(PowerPlantDto powerPlant, decimal windPercentage)
        {
            return powerPlant.Type switch
            {
                Constants.PowerPlantWindTurbine => decimal.Round(powerPlant.PMax / 100m * windPercentage, 1),
                _ => powerPlant.PMax
            };
        }

        /// <summary>
        /// Calculates the production cost (€/MWh) for a power plant using fuel price and efficiency.
        /// </summary>
        /// <param name="powerPlant">The power plant for which to compute cost; must include <see cref="PowerPlantDto.Efficiency"/> and <see cref="PowerPlantDto.Type"/>.</param>
        /// <param name="productionPlanParam">Production plan parameters containing fuel prices in <see cref="ProductionPlanParam.Fuels"/>.</param>
        /// <returns>
        /// Estimated cost in €/MWh for producing electricity from this plant. Returns 0 for plants without fuel cost (e.g., wind turbines).
        /// </returns>
        private static decimal CalculatePowerPlantCost(PowerPlantDto powerPlant, ProductionPlanParam productionPlanParam)
        {
            return powerPlant.Type switch
            {
                Constants.PowerPlantGasFired => productionPlanParam.Fuels.GasEuroPerMWh / powerPlant.Efficiency,
                Constants.PowerPlantTurboJet => productionPlanParam.Fuels.KerosineEuroPerMWh / powerPlant.Efficiency,
                _ => 0m
            };
        }
    }
}
