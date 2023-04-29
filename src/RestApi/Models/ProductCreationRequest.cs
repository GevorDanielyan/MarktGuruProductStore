namespace RestApi.Models
{
    public class ProductCreationRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string? Description { get; set; }
    }
}
