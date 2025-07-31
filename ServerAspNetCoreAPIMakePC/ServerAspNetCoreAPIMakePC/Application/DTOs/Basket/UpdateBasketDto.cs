namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateBasketDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ICollection<CreateBasketItemDto> Items { get; set; } = new List<CreateBasketItemDto>();
    }
}
