using System.Text.Json.Serialization;

namespace PowerPlantCodingChallenge.DTO
{
    public class PowerPlantFuelsDto
    {
        [JsonPropertyName("gas(euro/MWh)")]
        public decimal GasEuroPerMWh  { get;set; }

        [JsonPropertyName("kerosine(euro/MWh)")]
        public decimal KerosineEuroPerMWh { get; set; }

        [JsonPropertyName("co2(euro/ton)")]
        public int CO2EuroPerTon { get; set; }

        [JsonPropertyName("wind(%)")]
        public int WindPercentage { get; set; }
    }
}
