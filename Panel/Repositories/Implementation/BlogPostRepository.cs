using Microsoft.EntityFrameworkCore;
using Panel.Data;
using Panel.Models.Domain;
using Panel.Repositories.Interface;

namespace Panel.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext _db;

        public BlogPostRepository(AppDbContext db)
        {
            this._db = db;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await _db.BlogPosts.AddAsync(blogPost);
            await _db.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _db.BlogPosts.Include(x => x.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            return await _db.BlogPosts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlogPost = await _db.BlogPosts.Include(x => x.Categories).
                FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost == null) 
            {
               return null;
            }

            //update BlogPost 
            _db.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            //update Categories
            existingBlogPost.Categories = blogPost.Categories;

            await _db.SaveChangesAsync();

            return blogPost;
        }
    }
}
 