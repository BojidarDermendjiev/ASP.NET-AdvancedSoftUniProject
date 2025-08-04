namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class CategoryName : IEquatable<CategoryName>
    {
        public string Value { get; private set; }

        public CategoryName() { }
        public CategoryName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Category name is required.");
            if (value.Length < 2 || value.Length > 50)
                throw new ArgumentException("Category name must be 2-50 characters.");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as CategoryName);
        public bool Equals(CategoryName other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
