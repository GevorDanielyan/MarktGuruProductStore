namespace Domain.Exceptions.Structs
{
    public readonly struct AlreadyExistedProduct
    {
        private const string ErrorTemplate = "Product with that name {0} already exists";

        public AlreadyExistedProduct(string productName)
        {
            Message = string.Format(ErrorTemplate, productName);
        }

        public string Message { get; }
    }
}
