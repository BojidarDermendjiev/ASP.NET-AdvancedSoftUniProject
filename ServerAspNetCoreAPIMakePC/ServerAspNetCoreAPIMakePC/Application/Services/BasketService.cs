namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using DTOs.Basket;
    using ServerAspNetCoreAPIMakePC.Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public async Task<BasketDto?> GetByIdAsync(int id)
        {
            var basket = await this._basketRepository.GetByIdAsync(id);
            return basket is null ? null : this._mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto?> GetByUserIdAsync(Guid userId)
        {
            var basket = await this.GetByUserIdAsync(userId);
            return basket is null ? null : this._mapper.Map<BasketDto>(basket);
        }

        public async Task<IEnumerable<BasketDto>> GetAllAsync()
        {
            var baskets = await this._basketRepository.GetAllAsync();
            return baskets.Select(b => this._mapper.Map<BasketDto>(b));
        }

        public async Task<BasketDto> CreateAsync(CreateBasketDto dto)
        {
            var basket = this._mapper.Map<Domain.Entities.Basket>(dto);
            basket.DateCreated = DateTime.UtcNow;
            await this._basketRepository.AddAsync(basket);
            return this._mapper.Map<BasketDto>(basket);
        }

        public async Task<BasketDto> UpdateAsync(UpdateBasketDto dto)
        {
            var existingBasket = await this._basketRepository.GetByIdAsync(dto.Id);
            if (existingBasket is null)
            {
                throw new KeyNotFoundException(string.Format(InvalidBasketNotFound));
            }
            this._mapper.Map(dto, existingBasket);
            await this._basketRepository.UpdateAsync(existingBasket);
            return this._mapper.Map<BasketDto>(existingBasket);
        }

        public async Task DeleteAsync(int id)
        {
            var existingBasket = await this._basketRepository.GetByIdAsync(id);
            if (existingBasket is null)
            {
                throw new KeyNotFoundException(string.Format(InvalidBasketNotFound));
            }
            await this._basketRepository.DeleteAsync(id);   
        }
    }
}
