namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Feedback
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback;

    public class CreatePlatformFeedbackDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreatePlatformFeedbackDto
            {
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Great platform, very useful!"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Rating_Out_Of_Range_Fails_Validation()
        {
            var dto = new CreatePlatformFeedbackDto
            {
                UserId = Guid.NewGuid(),
                Rating = 100, 
                Comment = "Feedback"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("rating")));
        }

        [Test]
        public void Short_Comment_Fails_Validation()
        {
            var dto = new CreatePlatformFeedbackDto
            {
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