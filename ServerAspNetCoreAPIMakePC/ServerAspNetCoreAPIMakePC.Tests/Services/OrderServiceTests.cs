namespace ServerAspNetCoreAPIMakePC.Tests.Services
{
    using Moq;
    using AutoMapper;
    using NUnit.Framework;
    using Microsoft.Extensions.Options;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;
    using Application.Services;
    using Application.Settings;
    using Application.Interfaces;
    using Application.DTOs.Order;

    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private OrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            this._orderRepositoryMock = new Mock<IOrderRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._orderService = new OrderService(this._orderRepositoryMock.Object, this._mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsOrderDto_IfOrderExists()
        {
            var order = new Order { Id = 1 };
            var orderDto = new OrderDto { Id = 1 };

            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(order);
            this._mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            var result = await this._orderService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfOrderNotExists()
        {
            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Order)null);

            var result = await this._orderService.GetByIdAsync(1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsMappedOrderDtos()
        {
            var userId = Guid.NewGuid();
            var orders = new List<Order> { new Order { Id = 1, UserId = userId } };
            var orderDto = new OrderDto { Id = 1 };

            this._orderRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(orders);
            this._mapperMock.Setup(m => m.Map<OrderDto>(orders[0])).Returns(orderDto);

            var result = (await this._orderService.GetByUserIdAsync(userId)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedOrderDtos()
        {
            var orders = new List<Order> { new Order { Id = 1 }, new Order { Id = 2 } };
            var dto1 = new OrderDto { Id = 1 };
            var dto2 = new OrderDto { Id = 2 };

            this._orderRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(orders);
            this._mapperMock.Setup(m => m.Map<OrderDto>(orders[0])).Returns(dto1);
            this._mapperMock.Setup(m => m.Map<OrderDto>(orders[1])).Returns(dto2);

            var result = (await this._orderService.GetAllAsync()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsOrderDto()
        {
            var createDto = new CreateOrderDto();
            var order = new Order();
            var orderDto = new OrderDto();

            this._mapperMock.Setup(m => m.Map<Order>(createDto)).Returns(order);
            this._orderRepositoryMock.Setup(r => r.AddAsync(order)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            var result = await _orderService.CreateAsync(createDto);

            Assert.IsNotNull(result);
            this._orderRepositoryMock.Verify(r => r.AddAsync(order), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Throws_IfOrderNotFound()
        {
            var updateDto = new UpdateOrderDto { Id = 10 };
            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((Order)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _orderService.UpdateAsync(updateDto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsOrderDto()
        {
            var updateDto = new UpdateOrderDto { Id = 2 };
            var order = new Order { Id = 2 };
            var orderDto = new OrderDto { Id = 2 };

            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(order);
            this._mapperMock.Setup(m => m.Map(updateDto, order));
            this._orderRepositoryMock.Setup(r => r.UpdateAsync(order)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            var result = await _orderService.UpdateAsync(updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            _orderRepositoryMock.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Test]
        public async Task CreateOrderAsync_CreatesAndReturnsOrderDto()
        {
            var createDto = new CreateOrderDto();
            var order = new Order();
            var orderDto = new OrderDto();

            this._mapperMock.Setup(m => m.Map<Order>(createDto)).Returns(order);
            this._orderRepositoryMock.Setup(r => r.AddAsync(order)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<OrderDto>(order)).Returns(orderDto);

            var result = await _orderService.CreateOrderAsync(createDto);

            Assert.IsNotNull(result);
            this._orderRepositoryMock.Verify(r => r.AddAsync(order), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Throws_IfOrderNotFound()
        {
            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._orderService.DeleteAsync(21));
        }

        [Test]
        public async Task DeleteAsync_DeletesOrder_IfFound()
        {
            var order = new Order { Id = 7 };
            this._orderRepositoryMock.Setup(r => r.GetByIdAsync(order.Id)).ReturnsAsync(order);
            this._orderRepositoryMock.Setup(r => r.DeleteAsync(order.Id)).Returns(Task.CompletedTask);

            await this._orderService.DeleteAsync(order.Id);

            this._orderRepositoryMock.Verify(r => r.DeleteAsync(order.Id), Times.Once);
        }
    }
}