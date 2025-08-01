namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using DTOs.Basket;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Provides business logic for managing shopping baskets in the application.
    /// The BasketService class enables creating, retrieving (by basket ID or user ID), updating, and deleting baskets,
    /// as well as retrieving all baskets. It uses the basket repository for data access and AutoMapper for converting
    /// between entities and DTOs. Basket updates ensure that all basket items are replaced with those provided in the update DTO.
    /// Methods throw appropriate exceptions when baskets are not found.
    /// </summary>
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasketService"/> class with the specified basket repository and AutoMapper instance.
        /// </summary>
        /// <param name="basketRepository">The repository used for basket data operations.</param>
        /// <param name="mapper">The AutoMapper instance for mapping between entities and DTOs.</param>
        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            this._basketRepository = basketRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a basket by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the basket.</param>
        /// <returns>The corresponding basket DTO, or null if not found.</returns>
        public async Task<BasketDto?> GetByIdAsync(int id)
        {
            var basket = await this._basketRepository.GetByIdAsync(id);
            return basket is null ? null : this._mapper.Map<BasketDto>(basket);
        }

        /// <summary>
        /// Retrieves a basket for a specific user by user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>The basket DTO for the user, or null if not found.</returns>
        public async Task<BasketDto?> GetByUserIdAsync(Guid userId)
        {
            var basket = await this.GetByUserIdAsync(userId);
            return basket is null ? null : this._mapper.Map<BasketDto>(basket);
        }

        /// <summary>
        /// Retrieves all baskets.
        /// </summary>
        /// <returns>A collection of all basket DTOs.</returns>
        public async Task<IEnumerable<BasketDto>> GetAllAsync()
        {
            var baskets = await this._basketRepository.GetAllAsync();
            return baskets.Select(b => this._mapper.Map<BasketDto>(b));
        }

        /// <summary>
        /// Creates a new basket.
        /// </summary>
        /// <param name="dto">The DTO containing data for the new basket.</param>
        /// <returns>The created basket as a DTO.</returns>
        public async Task<BasketDto> CreateAsync(CreateBasketDto dto)
        {
            var basket = this._mapper.Map<Domain.Entities.Basket>(dto);
            basket.DateCreated = DateTime.UtcNow;
            await this._basketRepository.AddAsync(basket);
            return this._mapper.Map<BasketDto>(basket);
        }


        /// <summary>
        /// Updates an existing basket and its items.
        /// </summary>
        /// <param name="dto">The DTO containing updated basket data.</param>
        /// <returns>The updated basket as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the basket does not exist.</exception>
        public async Task<BasketDto> UpdateAsync(UpdateBasketDto dto)
        {
            var existing = await _basketRepository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException(string.Format(InvalidBasketNotFound));

            existing.Items.Clear();

            foreach (var itemDto in dto.Items)
            {
                var item = new BasketItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    BasketId = existing.Id
                };
                existing.Items.Add(item);
            }

            await _basketRepository.UpdateAsync(existing);
            return _mapper.Map<BasketDto>(existing);
        }

        /// <summary>
        /// Deletes a basket by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the basket to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the basket does not exist.</exception>
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
