namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using DTOs.Order;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for managing order operations.
    /// Provides business logic for creating, retrieving, updating, and deleting orders,
    /// as well as retrieving orders for specific users.
    /// Uses the order repository for persistence and AutoMapper for mapping between entities and DTOs.
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">The repository for order data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOs.</param>
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            this._orderRepository = orderRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The corresponding order DTO, or null if not found.</returns>
        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await this._orderRepository.GetByIdAsync(id);
            return order is null ? null : this._mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Retrieves all orders for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of order DTOs for the specified user.</returns>
        public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(Guid userId)
        {
            var orders = await this._orderRepository.GetByUserIdAsync(userId);
            return orders.Select(order => this._mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A collection of all order DTOs.</returns>
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await this._orderRepository.GetAllAsync();
            return orders.Select(order => this._mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="dto">The DTO containing data for the new order.</param>
        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            var order = this._mapper.Map<Domain.Entities.Order>(dto);
            order.OrderDate = DateTime.UtcNow;
            await this._orderRepository.AddAsync(order);
            return this._mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="dto">The DTO containing updated order data.</param>
        /// <returns>The updated order as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the order does not exist.</exception>
        public async Task<OrderDto> UpdateAsync(UpdateOrderDto dto)
        {
            var existingOrder = await this._orderRepository.GetByIdAsync(dto.Id);
            if (existingOrder is null)
            {
                throw new KeyNotFoundException(string.Format(OrderNotFound));
            }
            this._mapper.Map(dto, existingOrder);
            await this._orderRepository.UpdateAsync(existingOrder);
            return this._mapper.Map<OrderDto>(existingOrder);
        }

        /// <summary>
        /// Creates a new order based on the provided order data.
        /// </summary>
        /// <param name="dto">The DTO containing information required to create the order.</param>
        /// <returns>The created order as a DTO.</returns>
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = this._mapper.Map<Order>(dto);
            order.OrderDate = DateTime.UtcNow;
            await this._orderRepository.AddAsync(order);
            return this._mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Deletes an order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the order does not exist.</exception>
        public async Task DeleteAsync(int id)
        {
            var existingOrder = await this._orderRepository.GetByIdAsync(id);
            if (existingOrder is null)
            {
                throw new KeyNotFoundException(string.Format(OrderNotFound));
            }
            await this._orderRepository.DeleteAsync(id);
        }
    }
}