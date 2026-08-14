namespace BirdMessage.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Cep {get; set; }
        public string? Street {get; set; }
        public string? Neighborhood { get; set; }
        public string? Local { get; set; }
        public string? Uf { get; set; }
        public string? State { get; set; }
        public string? Region { get; set; }
        public string? DDD { get; set; }
    }
}