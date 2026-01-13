namespace PowerPlantCodingChallenge.DTO
{
    public class PowerPlantDto
    {
        public required string Name { get; set; }

        public required string Type { get; set; }

        public decimal Efficiency { get; set; }

        public int PMin { get; set; }

        public int PMax { get; set; }

        public decimal CostEuroPerMWh { get; set; }
    }
}
