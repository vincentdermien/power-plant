using PowerPlantCodingChallenge.DTO;

namespace PowerPlantCodingChallenge.BusinessContracts
{
    public interface IProductionPlanBusiness
    {
        ProductionPlanResult Calculate(ProductionPlanParam productionPlanParams);
    }
}
