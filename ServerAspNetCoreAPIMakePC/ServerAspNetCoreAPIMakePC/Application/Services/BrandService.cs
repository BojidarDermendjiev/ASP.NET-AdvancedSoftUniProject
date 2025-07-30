namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using DTOs.Brand;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            this._brandRepository = brandRepository;
            this._mapper = mapper;
        }
        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            var brand = await this._brandRepository.GetByIdAsync(id);
            return brand is null ? null : this._mapper.Map<BrandDto>(brand);
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await this._brandRepository.GetAllAsync();
            return brands.Select(b => this._mapper.Map<BrandDto>(b));
        }

        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            var brand = this._mapper.Map<Brand>(dto);
            await this._brandRepository.AddAsync(brand);
            return this._mapper.Map<BrandDto>(brand);
        }

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
