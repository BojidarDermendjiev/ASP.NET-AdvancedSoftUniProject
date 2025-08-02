namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    public class BasketDto
    {
        /// <summary>
        /// The unique identifier of the basket.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The unique identifier of the user who owns the basket.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The name of the user who owns the basket.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The date and time when the basket was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The collection of items contained in the basket.
        /// </summary>
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}
