namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Review
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Review;

    public class CreateReviewDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateReviewDto
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Rating = 5,
                Comment = "This is a valid review comment."
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Rating_Outside_Range_Fails_Validation()
        {
            var dto = new CreateReviewDto
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Rating = 100, 
                Comment = "Valid comment"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("rating")));
        }

        [Test]
        public void Short_Comment_Fails_Validation()
        {
            var dto = new CreateReviewDto
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "a" 
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("comment")));
        }
    }
}