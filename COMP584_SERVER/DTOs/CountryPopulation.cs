namespace COMP584_SERVER.DTOs
{
    public class CountryPopulation
    {
        public required string Name { get; set; }
        public int Id { get; set; }
        public required string Iso2 { get; set; }
        public required string Iso3 { get; set; }
        public decimal Population { get; set; }
    }
}
