namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
