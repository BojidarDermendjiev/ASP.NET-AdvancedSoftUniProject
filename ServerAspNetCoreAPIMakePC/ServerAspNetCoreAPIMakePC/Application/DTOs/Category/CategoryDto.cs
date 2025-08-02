namespace ServerAspNetCoreAPIMakePC.Application.DTOs.Category
{
    public class CategoryDto
    {
        /// <summary>
        /// The unique identifier of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The description of the category.
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
