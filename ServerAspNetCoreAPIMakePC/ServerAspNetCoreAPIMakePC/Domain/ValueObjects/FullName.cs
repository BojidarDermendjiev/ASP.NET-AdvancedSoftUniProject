namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class FullName : IEquatable<FullName>
    {
        public string Value { get; private set; }
        private FullName() { }
        public FullName(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 100)
                throw new ArgumentException("Full name must be 2-100 characters.");
            Value = value.Trim();
        }
        public override bool Equals(object obj) => Equals(obj as FullName);
        public bool Equals(FullName other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
