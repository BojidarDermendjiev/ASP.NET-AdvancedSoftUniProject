namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class Rating : IEquatable<Rating>
    {
        public int Value { get; private set; }

        private Rating() { }
        public Rating(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as Rating);
        public bool Equals(Rating other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static implicit operator int(Rating rating) => rating.Value;
        public static implicit operator Rating(int value) => new Rating(value);
    }
}
