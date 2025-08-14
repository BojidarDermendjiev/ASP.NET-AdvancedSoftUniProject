namespace ServerAspNetCoreAPIMakePC.Tests.Application.Mappings
{
    using System;
    using AutoMapper;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Domain.Enums;
    using ServerAspNetCoreAPIMakePC.Domain.Entities;
    using ServerAspNetCoreAPIMakePC.Domain.ValueObjects;
    using ServerAspNetCoreAPIMakePC.Application.Mappings;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Brand;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Review;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Basket;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Category;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;

    public class UserProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Test]
        public void Mapping_Configuration_Is_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            config.AssertConfigurationIsValid();
        }

        [Test]
        public void Can_Map_User_To_UserDto()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = new Email("test@example.com"),
                FullName = new FullName("Test User"),
                Role = UserRole.User
            };

            var dto = _mapper.Map<UserDto>(user);

            Assert.AreEqual(user.Id, dto.Id);
            Assert.AreEqual(user.Email.Value, dto.Email);
            Assert.AreEqual(user.FullName.Value, dto.FullName);
            Assert.AreEqual(user.Role.ToString(), dto.Role);
        }

        [Test]
        public void Can_Map_RegisterUserDto_To_User()
        {
            var registerDto = new RegisterUserDto
            {
                Email = "reg@example.com",
                Password = "Password123!",
                FullName = "Register User"
            };

            var user = _mapper.Map<User>(registerDto);

            Assert.AreEqual(registerDto.Email, user.Email.Value);
            Assert.AreEqual(registerDto.FullName, user.FullName.Value);
            Assert.AreEqual(UserRole.User, user.Role);
            Assert.IsNotNull(user.PasswordHash);
            Assert.IsNotNull(user.PasswordSalt);
        }

        [Test]
        public void Can_Map_UpdateUserDto_To_User_With_Only_NonNull_Fields()
        {
            var updateDto = new UpdateUserDto
            {
                Email = "new@email.com"
            };
            var user = new User { Email = new Email("old@email.com") };

            _mapper.Map(updateDto, user);

            Assert.AreEqual("new@email.com", user.Email.Value);
        }

        [Test]
        public void Can_Map_Basket_To_BasketDto()
        {
            var user = new User { FullName = new FullName("Basket Owner") };
            var basket = new Basket
            {
                Id = 1,
                User = user,
                DateCreated = DateTime.UtcNow,
                Items = new List<BasketItem>
                {
                    new BasketItem
                    {
                        Id = 2,
                        ProductId = Guid.NewGuid(),
                        Product = new Product { Name = new ProductName("Test Product") },
                        Quantity = new Quantity(2)
                    }
                }
            };

            var dto = _mapper.Map<BasketDto>(basket);

            Assert.AreEqual(basket.Id, dto.Id);
            Assert.AreEqual("Basket Owner", dto.UserName);
            Assert.AreEqual(basket.Items.Count, dto.Items.Count);
        }

        [Test]
        public void Can_Map_AddBasketItemDto_To_BasketItem()
        {
            var addBasketItemDto = new AddBasketItemDto
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 3
            };

            var basketItem = _mapper.Map<BasketItem>(addBasketItemDto);

            Assert.AreEqual(addBasketItemDto.ProductId, basketItem.ProductId);
            Assert.AreEqual(addBasketItemDto.Quantity, basketItem.Quantity.Value);
        }

        [Test]
        public void Can_Map_CreateCategoryDto_To_Category()
        {
            var dto = new CreateCategoryDto
            {
                Name = "Test Cat",
                Description = "Test Desc"
            };

            var category = _mapper.Map<Category>(dto);

            Assert.AreEqual(dto.Name, category.Name);
            Assert.AreEqual(dto.Description, category.Description);
        }

        [Test]
        public void Can_Map_Order_To_OrderDto()
        {
            var user = new User { FullName = new FullName("Order User") };
            var product = new Product { Name = new ProductName("ProductA") };
            var order = new Order
            {
                Id = 5,
                User = user,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 1,
                        Product = product,
                        Quantity = new Quantity(2),
                        UnitPrice = 10
                    }
                }
            };

            var dto = _mapper.Map<OrderDto>(order);

            Assert.AreEqual(order.Id, dto.Id);
            Assert.AreEqual("Order User", dto.UserName);
            Assert.AreEqual(order.Items.Count, dto.Items.Count);
            Assert.AreEqual("ProductA", ((List<OrderItemDto>)dto.Items)[0].ProductName);
        }

        [Test]
        public void Can_Map_Review_To_ReviewDto()
        {
            var review = new Review
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great!",
                Date = DateTime.UtcNow
            };

            var dto = _mapper.Map<ReviewDto>(review);

            Assert.AreEqual(review.Id, dto.Id);
            Assert.AreEqual(review.Rating, dto.Rating);
            Assert.AreEqual(review.Comment, dto.Comment);
        }

        [Test]
        public void Can_Map_PlatformFeedback_To_PlatformFeedbackDto()
        {
            var feedback = new PlatformFeedback
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = new FeedbackComment("very good application"),
                DateGiven = DateTime.UtcNow
            };

            var dto = _mapper.Map<PlatformFeedbackDto>(feedback);

            Assert.AreEqual(feedback.Id, dto.Id);
            Assert.AreEqual(feedback.Rating, dto.Rating);
            Assert.AreEqual(feedback.Comment, dto.Comment);
            Assert.AreEqual(feedback.DateGiven, dto.DateGiven);
        }

        [Test]
        public void Can_Map_Brand_To_BrandDto()
        {
            var brand = new Brand
            {
                Id = 1,
                Name = "MyBrand",
                Description = "Brand Desc",
                LogoUrl = "logo.png"
            };

            var dto = _mapper.Map<BrandDto>(brand);

            Assert.AreEqual(brand.Id, dto.Id);
            Assert.AreEqual(brand.Name, dto.Name);
            Assert.AreEqual(brand.Description, dto.Description);
            Assert.AreEqual(brand.LogoUrl, dto.LogoUrl);
        }
    }
}