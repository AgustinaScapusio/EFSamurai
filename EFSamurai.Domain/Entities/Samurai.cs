namespace EFSamurai.Domain.Entities
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Quote>? Quotes { get; set; }
        public ICollection<SamuraiBattle>? SamuraiBattles { get; set; }
        public HairStyle? HairStyle { get; set; }
        public SecretIdentity? SecretIdentity { get; set; } //Navigation property
    }
}
