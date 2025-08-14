namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Review
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Review;
    public class UpdateReviewDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateReviewDto
            {
                Id = 22,
                Rating = 4,
                Comment = "Updated comment text"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Rating_Too_Low_Fails_Validation()
        {
            var dto = new UpdateReviewDto
            {
                Id = 22,
                Rating = -1,
                Comment = "Valid comment"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("rating")));
        }

        [Test]
        public void Comment_Too_Short_Fails_Validation()
        {
            var dto = new UpdateReviewDto
            {
                Id = 22,
                Rating = 3,
                Comment = "" 
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("comment")));
        }
    }
}