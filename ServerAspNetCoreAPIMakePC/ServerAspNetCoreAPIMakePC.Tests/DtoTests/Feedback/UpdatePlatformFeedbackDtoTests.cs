namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Feedback
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback;
    public class UpdatePlatformFeedbackDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdatePlatformFeedbackDto
            {
                Id = 5,
                UserId = Guid.NewGuid(),
                Rating = 2,
                Comment = "Updated feedback comment"
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
            var dto = new UpdatePlatformFeedbackDto
            {
                Id = 5,
                UserId = Guid.NewGuid(),
                Rating = -1,
                Comment = "Feedback"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("rating")));
        }

        [Test]
        public void Comment_Too_Short_Fails_Validation()
        {
            var dto = new UpdatePlatformFeedbackDto
            {
                Id = 5,
                UserId = Guid.NewGuid(),
                Rating = 3,
                Comment = string.Empty
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("comment")));
        }
    }
}