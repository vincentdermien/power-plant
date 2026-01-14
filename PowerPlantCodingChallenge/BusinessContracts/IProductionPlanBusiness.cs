using PowerPlantCodingChallenge.DTO;

namespace PowerPlantCodingChallenge.BusinessContracts
{
    public interface IProductionPlanBusiness
    {
        List<ProductionPlanResult> Calculate(ProductionPlanParam productionPlanParams);
    }
}
