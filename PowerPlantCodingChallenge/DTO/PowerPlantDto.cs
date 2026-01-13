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
        public double Efficiency { get; set; }

        [Required]
        public int PMin { get; set; }

        [Required]
        public int PMax { get; set; }

        public double CostEuroPerMWh { get; set; }
    }
}
