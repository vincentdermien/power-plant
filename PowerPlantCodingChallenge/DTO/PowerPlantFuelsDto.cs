using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantCodingChallenge.DTO
{
    public class PowerPlantFuelsDto
    {
        [Required]
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasEuroPerMWh  { get;set; }

        [Required]
        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosineEuroPerMWh { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public decimal CO2EuroPerTon { get; set; }

        [Required]
        [JsonPropertyName("wind(%)")]
        public decimal WindPercentage { get; set; }
    }
}
