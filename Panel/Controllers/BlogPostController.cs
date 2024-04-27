using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Panel.Models.Domain;
using Panel.Models.DTO;
using Panel.Repositories.Interface;

namespace Panel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            this.categoryRepository = categoryRepository;
        }


        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDTO request)
        {
            var blogPost = new BlogPost
            {
                Title = request.Title,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Content = request.Content,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()

            };

            foreach (var categoryGuid in request.Categories) 
            {
             var existingCategory = await categoryRepository.GetById(categoryGuid);

                if(existingCategory is not null)
                {
                   blogPost.Categories.Add(existingCategory);
                }
            }

            blogPost = await blogPostRepository.CreateAsync(blogPost);

            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = request.Title,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDTO 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle= x.UrlHandle,
                }).ToList()    
                    
            };        
            
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            var response = new List<BlogPostDto>();
            foreach(var blogPost in blogPosts)
            {
                response.Add(new BlogPostDto
                { 
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    IsVisible=blogPost.IsVisible,
                    FeaturedImageUrl=blogPost.FeaturedImageUrl,
                    Categories = blogPost.Categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }

            return Ok(response);

        }

        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if(blogPost == null)
            {
                return NotFound();
            }

            //convert domain model to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDTO request)
        {
            //Convert DTO to Domain Model

            var blogPost = new BlogPost
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                FeaturedImageUrl = request.FeaturedImageUrl,
                IsVisible = request.IsVisible,
                Content = request.Content,
                PublishedDate = request.PublishedDate,
                ShortDescription = request.ShortDescription,
                UrlHandle = request.UrlHandle,
                Categories = new List<Category>()
            };

            //foreach
            foreach (var categoryGuid in request.Categories) 
            {
                var existingCategory = await categoryRepository.GetById(categoryGuid);

                if(existingCategory != null) 
                {
                  blogPost.Categories.Add(existingCategory);
                }
            }

            // call repository to update BlogPost domain model
            var updateBlogPost = await blogPostRepository.UpdateAsync(blogPost);
            
            if (updateBlogPost == null)
            {
                return NotFound();
            }

            //convert domain model back to DTO
            var response = new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Author = blogPost.Author,
                Content = blogPost.Content,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                IsVisible = blogPost.IsVisible,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Categories = blogPost.Categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(response);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deleteBlogPost = await blogPostRepository.DeleteAsync(id);  

            if (deleteBlogPost == null) {  return NotFound(); }

            //convert Domain model to DTO
            var response = new BlogPostDto
            {
                Id = deleteBlogPost.Id,
                Title = deleteBlogPost.Title,
                Author = deleteBlogPost.Author,
                Content = deleteBlogPost.Content,
                PublishedDate = deleteBlogPost.PublishedDate,
                ShortDescription = deleteBlogPost.ShortDescription,
                UrlHandle = deleteBlogPost.UrlHandle,
                IsVisible = deleteBlogPost.IsVisible,
                FeaturedImageUrl=deleteBlogPost.FeaturedImageUrl,
            };
            return Ok(response);

        }

    }
}
