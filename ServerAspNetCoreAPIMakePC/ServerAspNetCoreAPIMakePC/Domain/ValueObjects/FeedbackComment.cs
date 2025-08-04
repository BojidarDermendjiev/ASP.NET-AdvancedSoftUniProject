namespace ServerAspNetCoreAPIMakePC.Domain.ValueObjects
{
    public class FeedbackComment : IEquatable<FeedbackComment>
    {
        public string Value { get; private set; }
        public FeedbackComment() { }
        public FeedbackComment(string value)
        {
            if (value == null)
                throw new ArgumentException("Comment cannot be null.");
            if (value.Length > 1000)
                throw new ArgumentException("Comment too long (max 1000 characters).");
            Value = value;
        }
        public override bool Equals(object obj) => Equals(obj as FeedbackComment);
        public bool Equals(FeedbackComment other) => other != null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
