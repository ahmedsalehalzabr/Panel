using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Panel.Models.Domain;
using Panel.Models.DTO;
using Panel.Repositories.Interface;

namespace Panel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            // call image repository to get all image
            var images = await imageRepository.GetAll();

            //Convert domain model to DTO
            var response = new List<BlogImageDTO>();

            foreach (var image in images)
            {
                response.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtension = image.FileExtension,
                    FileName = image.FileName,
                    Url = image.Url,
                });
            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);
            if(ModelState.IsValid)
            {
                // file upload
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    Title = title,
                    FileName = fileName,
                    DateCreated = DateTime.Now,
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                //Convert Domain Model to DTO
                var response = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    FileName = blogImage.FileName,
                    DateCreated = blogImage.DateCreated,
                    FileExtension = blogImage.FileExtension,
                    Url = blogImage.Url

                };

                return Ok(blogImage);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtension.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be than 10MB");
            }
        }
    }
}
