namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class ProductName : IEquatable<ProductName>
    {
        public string Value { get; private set; }
        private ProductName() { }
        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name is required.");
            if (value.Length < 2 || value.Length > 100)
                throw new ArgumentException("Product name must be 2-100 characters.");
            Value = value.Trim();
        }
        public override bool Equals(object obj) => Equals(obj as ProductName);
        public bool Equals(ProductName other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
