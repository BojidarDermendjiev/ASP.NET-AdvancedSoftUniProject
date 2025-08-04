namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class ShippingAddress : IEquatable<ShippingAddress>
    {
        public string Value { get; private set; }

        private ShippingAddress() { } 
        public ShippingAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Shipping address is required.");
            if (value.Length < 5 || value.Length > 200)
                throw new ArgumentException("Shipping address must be 5-200 characters.");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as ShippingAddress);
        public bool Equals(ShippingAddress other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
