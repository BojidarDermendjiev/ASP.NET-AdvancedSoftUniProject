namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;

    using Application.Interfaces;
    using Application.DTOs.Review;
    using Microsoft.AspNetCore.Mvc;
    using static Domain.ErrorMessages.ErrorMessages;

    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this._reviewService = reviewService;
        }

        /// <summary>
        /// Gets all reviews.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            var reviews = await this._reviewService.GetAllAsync();
            if (!reviews.Any())
            {
                return NotFound(string.Format(ReviewNotFound));
            }

            return Ok(reviews);
        }

        /// <summary>
        /// Gets a review by its id.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            var review = await this._reviewService.GetByIdAsync(id);
            if (review is null)
            {
                return NotFound(string.Format(ReviewNotFound));
            }

            return Ok(review);
        }

        /// <summary>
        /// Gets all reviews for a specific product.
        /// </summary>
        [HttpGet("product/{productId:guid}")]
        [Authorize]
        public async Task<ActionResult> GetByProductId(Guid productId)
        {
            var reviews = await this._reviewService.GetByProductIdAsync(productId);
            if (!reviews.Any())
            {
                return NotFound(string.Format(ReviewNotFound));
            }

            return Ok(reviews);
        }

        /// <summary>
        /// Gets all reviews by a specific user.
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult> GetByUserId(Guid userId)
        {
            var reviews = await this._reviewService.GetByUserIdAsync(userId);
            if (!reviews.Any())
            {
                return NotFound(string.Format(ReviewNotFound));
            }

            return Ok(reviews);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create([FromBody] CreateReviewDto review)
        {
            if (review is null)
            {
                return BadRequest(string.Format(ReviewInvalid));
            }
        
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdReview = await this._reviewService.CreateAsync(review);
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto dto)
        {
            if (dto.Id != id)
            {
                return BadRequest(string.Format(ReviewIdMismatch));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedReview = await this._reviewService.UpdateAsync(dto);
                if (updatedReview is null)
                {
                    return NotFound(string.Format(ReviewNotFound));
                }

                return Ok(updatedReview);
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }

        }

        /// <summary>
        /// Deletes a review by its id.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await this._reviewService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
