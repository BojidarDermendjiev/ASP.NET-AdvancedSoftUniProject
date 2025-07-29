namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using Interfaces;
    using DTOs.Category;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = this._categoryRepository.GetByIdAsync(id);
            return category is null  ? null : this._mapper.Map<CategoryDto>(category);
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await this._categoryRepository.GetAllAsync();
            return this._mapper.Map<IEnumerable<CategoryDto>>(categories);
        }


        /// <summary>
        /// Creates a new category.
        /// </summary>
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = this._mapper.Map<Domain.Entities.Category>(dto);
            await this._categoryRepository.AddAsync(category);
            return this._mapper.Map<CategoryDto>(category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
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
        public async Task DeleteAsync(int id)
        {
            await this._categoryRepository.DeleteAsync(id);
        }
    }
}
