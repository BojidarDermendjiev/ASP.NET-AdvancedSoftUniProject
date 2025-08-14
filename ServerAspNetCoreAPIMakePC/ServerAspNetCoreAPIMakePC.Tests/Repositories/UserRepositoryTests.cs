namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.ValueObjects;
    using Infrastructure.Data;
    using Infrastructure.Repositories;

    [TestFixture]
    public class UserRepositoryTests
    {
        private MakePCDbContext _context;
        private UserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._userRepository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private User CreateTestUser(string email, string fullName)
        {
            var userId = Guid.NewGuid();
            return new User
            {
                Id = userId,
                Email = new Email(email),
                PasswordHash = "hashed_password",
                ConfirmPassword = "hashed_password",
                PasswordSalt = new byte[] { 1, 2, 3 },
                FullName = new FullName(fullName),
                Role = UserRole.User,
                ShoppingCart = new ShoppingCart { UserId = userId },
                Orders = new List<Order>(),
                Reviews = new List<Review>(),
                PlatformFeedbacks = new List<PlatformFeedback>()
            };
        }

        [Test]
        public async Task AddAsync_AddsUser()
        {
            var user = CreateTestUser("test@example.com", "Test User");

            await this._userRepository.AddAsync(user);

            var stored = await this._context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual("test@example.com", stored.Email.ToString());
            Assert.AreEqual("Test User", stored.FullName.ToString());
        }

        [Test]
        public async Task GetByEmailAsync_ReturnsUser_IfExists()
        {
            var user = CreateTestUser("exists@example.com", "Exists User");
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            var found = await this._userRepository.GetByEmailAsync(new Email("exists@example.com"));
            Assert.IsNotNull(found);
            Assert.AreEqual("Exists User", found.FullName.ToString());
        }

        [Test]
        public async Task GetByEmailAsync_ReturnsNull_IfNotExists()
        {
            var found = await this._userRepository.GetByEmailAsync(new Email("none@example.com"));
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsUser_IfExists()
        {
            var user = CreateTestUser("byid@example.com", "ById User");
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            var found = await this._userRepository.GetByIdAsync(user.Id);
            Assert.IsNotNull(found);
            Assert.AreEqual("ById User", found.FullName.ToString());
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotExists()
        {
            var found = await this._userRepository.GetByIdAsync(Guid.NewGuid());
            Assert.IsNull(found);
        }

        [Test]
        public async Task UpdateAsync_UpdatesUser()
        {
            var user = CreateTestUser("update@example.com", "Before Update");
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            user.FullName = new FullName("After Update");
            await this._userRepository.UpdateAsync(user);

            var updated = await this._context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.AreEqual("After Update", updated.FullName.ToString());
        }

        [Test]
        public void UpdateAsync_Throws_IfUserIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._userRepository.UpdateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_DeletesUser_IfExists()
        {
            var user = CreateTestUser("delete@example.com", "Delete User");
            this._context.Users.Add(user);
            await this._context.SaveChangesAsync();

            await this._userRepository.DeleteAsync(user.Id);

            var deleted = await this._context.Users.FindAsync(user.Id);
            Assert.IsNull(deleted);
        }

        [Test]
        public void DeleteAsync_Throws_IfNotExists()
        {
            var nonExistentId = Guid.NewGuid();
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._userRepository.DeleteAsync(nonExistentId));
            Assert.That(ex.Message, Does.Contain(nonExistentId.ToString()));
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            this._context.Users.Add(CreateTestUser("a@a.com", "Ivan Shishkov"));
            this._context.Users.Add(CreateTestUser("b@b.com", "Geaory Joe"));
            await this._context.SaveChangesAsync();

            var all = await this._userRepository.GetAllAsync();
            Assert.AreEqual(2, all.Count);
        }
    }
}