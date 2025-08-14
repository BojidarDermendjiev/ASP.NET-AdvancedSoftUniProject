namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using System;
    using System.Linq;
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
    public class PlatformFeedbackRepositoryTests
    {
        private MakePCDbContext _context;
        private PlatformFeedbackRepository _repository;
        private int _userSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new PlatformFeedbackRepository(this._context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private User CreateUser(Guid? id = null)
        {
            var userId = id ?? Guid.NewGuid();
            return new User
            {
                Id = userId,
                Email = new Email($"user{_userSeq++}@test.com"),
                PasswordHash = "hash",
                ConfirmPassword = "hash",
                PasswordSalt = new byte[] { 1, 2, 3 },
                FullName = new FullName("Test User"),
                Role = UserRole.User,
                ShoppingCart = new ShoppingCart { UserId = userId }
            };
        }

        private PlatformFeedback CreateFeedback(int? id = null, Guid? userId = null, string? content = null)
        {
            var user = CreateUser(userId);
            this._context.Users.Add(user);
            this._context.SaveChanges();
            return new PlatformFeedback
            {
                Id = id ?? 0,
                UserId = user.Id,
                User = user,
                Comment = new FeedbackComment(content ?? "Great platform!"),
                DateGiven = DateTime.UtcNow
            };
        }

        [Test]
        public async Task AddAsync_AddsFeedback()
        {
            var feedback = CreateFeedback(1);

            await this._repository.AddAsync(feedback);

            var stored = await this._context.PlatformFeedbacks.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == feedback.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual(feedback.Comment, stored.Comment);
            Assert.IsNotNull(stored.User);
        }

        [Test]
        public void AddAsync_Throws_IfFeedbackIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsFeedback_WhenExists()
        {
            var feedback = CreateFeedback(2);
            this._context.PlatformFeedbacks.Add(feedback);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(feedback.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(feedback.Id, found.Id);
            Assert.IsNotNull(found.User);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(-999);
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllFeedbacks()
        {
            this._context.PlatformFeedbacks.Add(CreateFeedback(3));
            this._context.PlatformFeedbacks.Add(CreateFeedback(4));
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
            Assert.IsNotNull(all[0].User);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsFeedbacksForUser()
        {
            var userId = Guid.NewGuid();
            var feedback1 = CreateFeedback(5, userId);
            var feedback2 = CreateFeedback(6, userId);
            this._context.PlatformFeedbacks.AddRange(feedback1, feedback2);
            this._context.PlatformFeedbacks.Add(CreateFeedback(7)); 
            await _context.SaveChangesAsync();

            var feedbacks = (await this._repository.GetByUserIdAsync(userId)).ToList();

            Assert.AreEqual(2, feedbacks.Count);
            Assert.IsTrue(feedbacks.All(f => f.UserId == userId));
            Assert.IsNotNull(feedbacks[0].User);
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingFeedback()
        {
            var feedback = CreateFeedback(8);
            this._context.PlatformFeedbacks.Add(feedback);
            await this._context.SaveChangesAsync();

            feedback.Comment = new FeedbackComment("Updated feedback!");
            await this._repository.UpdateAsync(feedback);

            var updated = await this._context.PlatformFeedbacks.FirstOrDefaultAsync(f => f.Id == feedback.Id);
            Assert.AreEqual("Updated feedback!", updated.Comment.Value);
        }

        [Test]
        public async Task DeleteAsync_RemovesFeedback_WhenExists()
        {
            var feedback = CreateFeedback(9);
            this._context.PlatformFeedbacks.Add(feedback);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(feedback.Id);

            var found = await this._context.PlatformFeedbacks.FindAsync(feedback.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => this._repository.DeleteAsync(-1234));
        }
    }
}