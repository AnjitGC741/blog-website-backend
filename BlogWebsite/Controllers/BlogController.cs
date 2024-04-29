using BlogWebsite.Data;
using BlogWebsite.Models;
using BlogWebsite.Models.FormInput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public BlogController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }
        [HttpPost]
        [Route("create-blog")]
        public IActionResult Create([FromForm] BlogModel blog)
        {
            try
            {
                Blog createBlog = new Blog();
                createBlog.Title = blog.Title;
                createBlog.Description = blog.Description;
                createBlog.UserId = blog.UserId;
                createBlog.BlogTypeId = blog.BlogTypeId;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (blog.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blog.Image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Image");
                    string filePath = Path.Combine(productPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        blog.Image.CopyTo(fileStream);
                    }

                    createBlog.ImagePath = @"/Image/" + fileName;
                }
                _dbContext.Blogs.Add(createBlog);
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }


        [HttpPut]
        [Route("edit-blog/{Id}")]
        public IActionResult Edit(int Id, [FromForm] BlogModel blog)
        {
            try
            {
                var editBlog = _dbContext.Blogs.Find(Id);
                if (editBlog == null)
                {
                    return StatusCode(500, "Blog cannot be found");
                }
                editBlog.Title = blog.Title;
                editBlog.Description = blog.Description;
                editBlog.UserId = blog.UserId;
                editBlog.BlogTypeId = blog.BlogTypeId;
                editBlog.IsEdited = true;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (blog.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(blog.Image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Image");
                    string filePath = Path.Combine(productPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        blog.Image.CopyTo(fileStream);
                    }

                    editBlog.ImagePath = @"/Image/" + fileName;
                }
                _dbContext.Blogs.Update(editBlog);
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog edited Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });

            }

        }

        [HttpDelete]
        [Route("delete-blog/{Id}")]
        public IActionResult Delete(int Id)
        {
            try
            {
                var editBlog = _dbContext.Blogs.Find(Id);
                if (editBlog == null)
                {
                    return StatusCode(500, "Blog cannot be found");
                }
                _dbContext.Blogs.Remove(editBlog);
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog Deleted Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPut]
        [Route("like-blog/{Id}")]
        public IActionResult Like(int Id) {
            try
            {
                var likeBlog = _dbContext.Blogs.Find(Id);
                if (likeBlog == null)
                {
                    return StatusCode(500, "Blog cannot be found");
                }
                likeBlog.UpVote = (likeBlog.UpVote == null ? 1 : likeBlog.UpVote + 1);
                _dbContext.Blogs.Update(likeBlog);
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog Liked Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpPut]
        [Route("dislike-blog/{Id}")]
        public IActionResult Dislike(int Id)
        {
            try
            {
                var likeBlog = _dbContext.Blogs.Find(Id);
                if (likeBlog == null)
                {
                    return StatusCode(500, "Blog cannot be found");
                }
                likeBlog.UpVote = (likeBlog.UpVote != null ? likeBlog.UpVote - 1 : null);
                _dbContext.Blogs.Update(likeBlog);
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog DisLiked Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("blog-list")]
        public IActionResult GetAllBlogList()
        {
            try
            {

                var blogList = _dbContext.Blogs.ToList();
                return StatusCode(200, blogList);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("blog/{Id}")]
        public IActionResult GetBlogDetail(int Id)
        {
            try
            {
                var blogList = _dbContext.Blogs.Where(x=>x.Id == Id);
                if(blogList.Any())
                {
                return StatusCode(200, blogList);

                }
                return StatusCode(500, "Blog cannot be found");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("blogger-blog/{Id}")]
        public IActionResult GetBlogOfBlogger(int Id)
        {
            try
            {
                var blogList = _dbContext.Blogs.Where(x => x.UserId == Id);
                if (blogList.Any())
                {
                    return StatusCode(200, blogList);

                }
                return StatusCode(500, "Blog cannot be found");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
