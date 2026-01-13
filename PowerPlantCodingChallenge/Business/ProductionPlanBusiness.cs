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

            var producedPower = 0d;

            foreach (var powerPlant in productionPlanParams.PowerPlants.OrderBy(x => x.CostEuroPerMWh))
            {
                var stillNeededPower = productionPlanParams.Load - producedPower;

                if (stillNeededPower == 0d)
                {
                    productionPlanResult.ProductionPlanResultItems.Add(new ProductionPlanResultItem()
                    {
                        PowerPlantName = powerPlant.Name,
                        Power = 0d
                    });
                    continue;
                }

                if (powerPlant.PMin > stillNeededPower)
                    continue;

                var availablePower = calculatePowerPlantAvailablePower(powerPlant, productionPlanParams.Fuels.WindPercentage);
                var usedPower = double.Min(stillNeededPower, availablePower);
                productionPlanResult.ProductionPlanResultItems.Add(new ProductionPlanResultItem()
                {
                    PowerPlantName = powerPlant.Name,
                    Power = usedPower
                });

                producedPower += usedPower;
            }

            return productionPlanResult;
        }

        private double calculatePowerPlantAvailablePower(PowerPlantDto powerPlant, int windPercentage)
        {
            switch (powerPlant.Type)
            {
                case Constants.PowerPlantWindTurbine:
                    return double.Round((powerPlant.PMax / 100d) * windPercentage, 1);
                default:
                    return powerPlant.PMax;
            }
        }

        private void calculatePowerPlantCost(ProductionPlanParam productionPlanParam)
        {
            foreach (var powerPlant in productionPlanParam.PowerPlants)
            {
                switch (powerPlant.Type)
                {
                    case Constants.PowerPlantWindTurbine:
                        powerPlant.CostEuroPerMWh = 0;
                        break;
                    case Constants.PowerPlantGasFired:
                        powerPlant.CostEuroPerMWh = productionPlanParam.Fuels.GasEuroPerMWh / powerPlant.Efficiency;
                        break;
                    case Constants.PowerPlantTurboJet:
                        powerPlant.CostEuroPerMWh = productionPlanParam.Fuels.KerosineEuroPerMWh / powerPlant.Efficiency;
                        break;
                }
            }
        }
    }
}
