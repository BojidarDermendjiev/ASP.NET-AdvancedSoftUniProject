namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Review
{
    public class ReviewDto
    {
        /// <summary>
        /// The unique identifier of the review.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who wrote the review.
        /// </summary>
        public Guid UserId { get; set; }


        /// <summary>
        /// The unique identifier of the product being reviewed. Optional field.
        /// </summary>
        public Guid? ProductId { get; set; }

        /// <summary>
        /// The rating value given in the review.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// The comment or message provided in the review.
        /// </summary>
        public string Comment { get; set; } = null!;

        /// <summary>
        /// The date and time when the review was created.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The name of the user who wrote the review. Optional field.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// The name of the product being reviewed. Optional field.
        /// </summary>
        public string? ProductName { get; set; }
    }
}
