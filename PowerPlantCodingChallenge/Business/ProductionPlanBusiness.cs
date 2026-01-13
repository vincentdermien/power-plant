using PowerPlantCodingChallenge.BusinessContracts;
using PowerPlantCodingChallenge.DTO;
using PowerPlantCodingChallenge.Utils;

namespace PowerPlantCodingChallenge.Business
{
    public class ProductionPlanBusiness : IProductionPlanBusiness
    {
        public ProductionPlanResult Calculate(ProductionPlanParam productionPlanParams)
        {
            var productionPlanResult = new ProductionPlanResult() { ProductionPlanResultItems = new List<ProductionPlanResultItem>() };
            calculatePowerPlantCost(productionPlanParams);

            var givenPower = decimal.Zero;
            var usedPowerPlant = new HashSet<string>();

            foreach (var powerPlant in productionPlanParams.PowerPlants.OrderBy(x => x.CostEuroPerMWh))
            {
                var stillNeededPower = productionPlanParams.Load - givenPower;

                if (stillNeededPower == decimal.Zero)
                {
                    productionPlanResult.ProductionPlanResultItems.Add(new ProductionPlanResultItem()
                    {
                        PowerPlantName = powerPlant.Name,
                        Power = decimal.Zero
                    });
                    continue;
                }

                if (powerPlant.PMin > stillNeededPower)
                    continue;

                var availablePower = calculatePowerPlantAvailablePower(powerPlant, productionPlanParams.Fuels.WindPercentage);
                var usedPower = decimal.Min(stillNeededPower, availablePower);
                productionPlanResult.ProductionPlanResultItems.Add(new ProductionPlanResultItem()
                {
                    PowerPlantName = powerPlant.Name,
                    Power = usedPower
                });

                givenPower += usedPower;
            }

            return productionPlanResult;
        }

        private decimal calculatePowerPlantAvailablePower(PowerPlantDto powerPlant, int windPercentage)
        {
            switch (powerPlant.Type)
            {
                case Constants.PowerPlantWindTurbine:
                    return decimal.Round((powerPlant.PMax / 100m) * windPercentage, 1);
                default:
                    return powerPlant.PMax;
            }   
        }

        private void calculatePowerPlantCost(ProductionPlanParam productionPlanParam)
        {
            foreach (var powerPlant in productionPlanParam.PowerPlants)
            {
                if (powerPlant.Type == Constants.PowerPlantWindTurbine)
                    powerPlant.CostEuroPerMWh = 0;

                if (powerPlant.Type == Constants.PowerPlantGasFired)
                    powerPlant.CostEuroPerMWh = productionPlanParam.Fuels.GasEuroPerMWh / powerPlant.Efficiency;

                if (powerPlant.Type == Constants.PowerPlantTurboJet)
                    powerPlant.CostEuroPerMWh = productionPlanParam.Fuels.KerosineEuroPerMWh / powerPlant.Efficiency;
            }
        }
    }
}
