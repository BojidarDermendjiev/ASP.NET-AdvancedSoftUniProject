namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using DTOs.Order;
    using Interfaces;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            this._orderRepository = orderRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await this._orderRepository.GetByIdAsync(id);
            return order is null ? null : this._mapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Retrieves all orders for a specific user.
        /// </summary>
        public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(Guid userId)
        {
            var orders = await this._orderRepository.GetByUserIdAsync(userId);
            return orders.Select(order => this._mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await this._orderRepository.GetAllAsync();
            return orders.Select(order => this._mapper.Map<OrderDto>(order));
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
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
        /// Deletes an order by its unique identifier.
        /// </summary>
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
