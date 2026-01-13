namespace PowerPlantCodingChallenge.DTO
{
    public class ProductionPlanParam
    {
        public decimal Load { get; set; }

        public required PowerPlantFuelsDto Fuels { get; set; }

        public required List<PowerPlantDto> PowerPlants { get; set; }
    }
}
