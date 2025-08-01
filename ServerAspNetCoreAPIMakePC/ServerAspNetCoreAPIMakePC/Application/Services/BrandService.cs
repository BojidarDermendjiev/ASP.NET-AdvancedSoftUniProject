namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using DTOs.Brand;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for managing brand operations.
    /// Provides business logic for creating, retrieving, updating, and deleting brands.
    /// Utilizes the brand repository for data persistence and AutoMapper for mapping between entities and DTOs.
    /// </summary>
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandService"/> class.
        /// </summary>
        /// <param name="brandRepository">Repository for brand data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOs.</param>
        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            this._brandRepository = brandRepository;
            this._mapper = mapper;
        }


        /// <summary>
        /// Retrieves a brand by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the brand.</param>
        /// <returns>The corresponding brand DTO, or null if not found.</returns>
        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await this._brandRepository.GetByIdAsync(id);
            return brand is null ? null : this._mapper.Map<BrandDto>(brand);
        }

        /// <summary>
        /// Retrieves all brands.
        /// </summary>
        /// <returns>A collection of all brand DTOs.</returns>
        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await this._brandRepository.GetAllAsync();
            return brands.Select(b => this._mapper.Map<BrandDto>(b));
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="dto">The DTO containing data for the new brand.</param>
        /// <returns>The created brand as a DTO.</returns>
        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            var brand = this._mapper.Map<Brand>(dto);
            await this._brandRepository.AddAsync(brand);
            return this._mapper.Map<BrandDto>(brand);
        }

        /// <summary>
        /// Updates an existing brand.
        /// </summary>
        /// <param name="dto">The DTO containing updated brand data.</param>
        /// <returns>The updated brand as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the brand does not exist.</exception>
        public async Task<BrandDto> UpdateAsync(UpdateBrandDto dto)
        {
            var existing = await this._brandRepository.GetByIdAsync(dto.Id);
            if (existing is null)
            {
                throw new KeyNotFoundException(InvalidBrandNotFound);
            }

            this._mapper.Map(dto, existing);
            await this._brandRepository.UpdateAsync(existing);
            return this._mapper.Map<BrandDto>(existing);
        }

        /// <summary>
        /// Deletes a brand by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the brand to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the brand does not exist.</exception>
        public async Task DeleteAsync(int id)
        {
            var existing = await this._brandRepository.GetByIdAsync(id);
            if (existing is null)
            {
                throw new KeyNotFoundException(InvalidBrandNotFound);
            }

            await this._brandRepository.DeleteAsync(id);
        }
    }
}
