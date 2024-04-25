using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Panel.Data;
using Panel.Models.Domain;
using Panel.Models.DTO;
using Panel.Repositories.Interface;

namespace Panel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
          this.categoryRepository = categoryRepository;
         
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreatecategoryRequestDto request)
        {
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

           await categoryRepository.CreateAsync(category);

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            var response = new List<CategoryDTO>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDTO
                {
                    Id=category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                });
            }

            return Ok(response);
        }
    }
}
