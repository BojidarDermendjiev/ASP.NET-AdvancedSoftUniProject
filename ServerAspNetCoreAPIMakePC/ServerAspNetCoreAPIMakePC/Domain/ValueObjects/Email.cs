namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    using System.Text.RegularExpressions;
    public class Email : IEquatable<Email>
    {
        public string Value { get; private set; }

        private static readonly Regex EmailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private Email() { }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");
            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("Invalid email format.");
            Value = value.Trim().ToLowerInvariant();
        }
        public override string ToString() => Value;
        public override bool Equals(object obj) => Equals(obj as Email);
        public bool Equals(Email other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public static bool operator ==(Email left, Email right) => Equals(left, right);
        public static bool operator !=(Email left, Email right) => !Equals(left, right);
    }
}
