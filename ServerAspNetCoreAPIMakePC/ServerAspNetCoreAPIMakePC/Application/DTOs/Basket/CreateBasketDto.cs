namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Basket
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;

    public class CreateBasketDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public ICollection<CreateBasketItemDto> Items { get; set; } = new List<CreateBasketItemDto>();
    }
}
