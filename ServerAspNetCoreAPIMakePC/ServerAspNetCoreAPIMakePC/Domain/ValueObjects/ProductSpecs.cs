namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class ProductSpecs : IEquatable<ProductSpecs>
    {
        public string Value { get; private set; }
        private ProductSpecs() { }
        public ProductSpecs(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Specs cannot be empty.");
            Value = value.Trim();
        }
        public override bool Equals(object obj) => Equals(obj as ProductSpecs);
        public bool Equals(ProductSpecs other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
