namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class BrandName : IEquatable<BrandName>
    {
        public string Value { get; private set; }
        private BrandName() { }
        public BrandName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Brand name is required.");
            if (value.Length is < 2 or > 50)
                throw new ArgumentException("Brand name must be 2-50 characters.");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as BrandName);
        public bool Equals(BrandName other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
        public static implicit operator string(BrandName brandName) => brandName?.Value;
        public static implicit operator BrandName(string value) => value == null ? null : new BrandName(value);
    }
}
