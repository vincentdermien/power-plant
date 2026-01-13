using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlantCodingChallenge.DTO
{
    public class PowerPlantFuelsDto
    {
        [Required]
        [JsonPropertyName("gas(euro/MWh)")]
        public double GasEuroPerMWh  { get;set; }

        [Required]
        [JsonPropertyName("kerosine(euro/MWh)")]
        public double KerosineEuroPerMWh { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public int CO2EuroPerTon { get; set; }

        [Required]
        [JsonPropertyName("wind(%)")]
        public int WindPercentage { get; set; }
    }
}
