namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using Interfaces;
    using DTOs.Category;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for managing category operations.
    /// Provides business logic for creating, retrieving, updating, and deleting product categories.
    /// Utilizes the category repository for data persistence and AutoMapper for mapping between entities and DTOs.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The repository for category data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOsa.</param>
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>The corresponding category DTO, or null if not found.</returns>
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = this._categoryRepository.GetByIdAsync(id);
            return category is null  ? null : this._mapper.Map<CategoryDto>(category);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A collection of all category DTOs.</returns>
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await this._categoryRepository.GetAllAsync();
            return this._mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="dto">The DTO containing data for the new category.</param>
        /// <returns>The created category as a DTO.</returns>
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = this._mapper.Map<Domain.Entities.Category>(dto);
            await this._categoryRepository.AddAsync(category);
            return this._mapper.Map<CategoryDto>(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="dto">The DTO containing updated category data.</param>
        /// <returns>The updated category as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the category does not exist.</exception>
        public async Task<CategoryDto> UpdateAsync(UpdateCategoryDto dto)
        {
            var existingCategory = await this._categoryRepository.GetByIdAsync(dto.Id);
            if (existingCategory is null)
            {
                throw new KeyNotFoundException(string.Format(CategoryNotFound));
            }
            this._mapper.Map(dto, existingCategory);
            await this._categoryRepository.UpdateAsync(existingCategory);
            return this._mapper.Map<CategoryDto>(existingCategory);
        }

        /// <summary>
        /// Deletes a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        public async Task DeleteAsync(int id)
        {
            await this._categoryRepository.DeleteAsync(id);
        }
    }
}
