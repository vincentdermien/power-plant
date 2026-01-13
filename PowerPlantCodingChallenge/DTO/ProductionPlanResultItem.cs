using System.Text.Json.Serialization;

namespace PowerPlantCodingChallenge.DTO
{
    public class ProductionPlanResultItem
    {
        [JsonPropertyName("name")]
        public required string PowerPlantName { get; set; }

        [JsonPropertyName("p")]
        public double Power { get; set; }
    }
}
