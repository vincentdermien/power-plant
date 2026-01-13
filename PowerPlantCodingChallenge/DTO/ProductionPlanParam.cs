using System.ComponentModel.DataAnnotations;

namespace PowerPlantCodingChallenge.DTO
{
    public class ProductionPlanParam
    {
        [Required]
        [Range(0d, double.MaxValue)]
        public double Load { get; set; }

        [Required]
        public required PowerPlantFuelsDto Fuels { get; set; }

        [Required]
        public required List<PowerPlantDto> PowerPlants { get; set; }
    }
}
