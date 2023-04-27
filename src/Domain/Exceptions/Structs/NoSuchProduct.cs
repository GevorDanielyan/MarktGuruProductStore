namespace Domain.Exceptions.Structs
{
    public readonly struct NoSuchProduct
    {
        private const string ErrorTemplate = "There is no such product with this id {0}";

        public NoSuchProduct(Guid id)
        {
            Message = string.Format(ErrorTemplate, id);
        }

        public string Message { get; }
    }
}
