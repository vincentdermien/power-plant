using System.ComponentModel.DataAnnotations;

namespace PowerPlantCodingChallenge.DTO
{
    public class PowerPlantDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public decimal Efficiency { get; set; }

        [Required]
        public decimal PMin { get; set; }

        [Required]
        public decimal PMax { get; set; }

        public decimal CostEuroPerMWh { get; set; }
    }
}
