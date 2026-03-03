using MultiShop.Catalog.Dtos.CategoryDtos;

namespace MultiShop.Catalog.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();


        //Bu method async çalışır ama geriye değer döndürmez.
        //Yani async versiyonu void gibi düşünebilirsin. return olmaz 
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task  DeleteCategoryAsync(string id);
        Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id);


    }
}
