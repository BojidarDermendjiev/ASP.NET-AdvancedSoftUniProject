namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    public class BasketDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public ICollection<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
    }
}
