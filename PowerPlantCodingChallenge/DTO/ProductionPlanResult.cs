using System.Text.Json.Serialization;

namespace PowerPlantCodingChallenge.DTO
{
    public class ProductionPlanResult
    {
        [JsonPropertyName("name")]
        public required string PowerPlantName { get; set; }

        [JsonPropertyName("p")]
        public decimal Power { get; set; }

        [JsonIgnore]
        public decimal CostEuroPerMWh { get; set; }

        [JsonIgnore]
        public decimal PMin { get; set; }
    }
}
