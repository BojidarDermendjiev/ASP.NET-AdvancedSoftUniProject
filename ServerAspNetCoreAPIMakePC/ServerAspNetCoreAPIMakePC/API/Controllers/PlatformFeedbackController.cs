namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using Application.Interfaces;
    using Application.DTOs.Feedback;
    using static Domain.ErrorMessages.ErrorMessages;

    public class PlatformFeedbackController : ControllerBase
    {
        private readonly IPlatformFeedbackService _platformFeedbackService;

        public PlatformFeedbackController(IPlatformFeedbackService platformFeedbackService)
        {
            this._platformFeedbackService = platformFeedbackService;
        }


        /// <summary>
        /// Retrieves all platform feedback entries.
        /// GET /api/platformfeedback
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var feedbacks = await this._platformFeedbackService.GetAllAsync();
            return Ok(feedbacks);
        }

        /// <summary>
        /// Retrieves a specific feedback entry by its ID.
        /// GET /api/platformfeedback/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await this._platformFeedbackService.GetByIdAsync(id);
            if (feedback is null)
            {
                return NotFound();
            }
            return Ok(feedback);
        }

        /// <summary>
        /// Retrieves all feedback entries for a specific user.
        /// GET /api/platformfeedback/user/{userId}
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var feedbacks = await this._platformFeedbackService.GetByUserIdAsync(userId);
            if (!feedbacks.Any())
            {
                return NotFound();
            }
            return Ok(feedbacks);
        }

        /// <summary>
        /// Creates a new platform feedback entry.
        /// POST /api/platformfeedback
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlatformFeedbackDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await this._platformFeedbackService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        /// <summary>
        /// Updates an existing feedback entry.
        /// PUT /api/platformfeedback/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePlatformFeedbackDto dto)
        {
            if (id != dto.Id)
                return BadRequest(string.Format(IdMismatch));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await this._platformFeedbackService.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a feedback entry by its ID.
        /// DELETE /api/platformfeedback/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this._platformFeedbackService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
