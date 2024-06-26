﻿using Panel.Models.Domain;

namespace Panel.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost>  CreateAsync(BlogPost blogPost);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task<BlogPost?> GetByIdAsync(Guid id); 
        Task<BlogPost?> GetByIdHandleAsync(string urlHandle);

        Task<BlogPost?> UpdateAsync(BlogPost blogPost);

        Task<BlogPost?> DeleteAsync(Guid id);
    }
}
