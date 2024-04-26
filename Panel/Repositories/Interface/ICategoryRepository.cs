using Panel.Models.Domain;

namespace Panel.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id);

        Task<Category> UpdateAsync(Category category);

        Task<Category?> DeleteAsync(Guid id);

        

    }
}
