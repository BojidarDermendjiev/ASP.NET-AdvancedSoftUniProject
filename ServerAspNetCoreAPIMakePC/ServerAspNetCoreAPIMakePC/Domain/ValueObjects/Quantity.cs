namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class Quantity : IEquatable<Quantity>
    {
        public int Value { get; private set; }
        private Quantity() { }
        public Quantity(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as Quantity);
        public bool Equals(Quantity other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Quantity left, Quantity right) => Equals(left, right);
        public static bool operator !=(Quantity left, Quantity right) => !Equals(left, right);
        public override string ToString() => Value.ToString();
    }
}
