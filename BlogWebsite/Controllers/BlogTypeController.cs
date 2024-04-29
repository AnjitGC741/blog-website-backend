using BlogWebsite.Data;
using BlogWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public BlogTypeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [Route("seed-blogType")]
        public IActionResult SeedBlogType()
        {
            try
            {
                string[] blogTypeList = {"Travel", "Food","Education"};
                if(_dbContext.BlogTypes.Any())
                {
                    _dbContext.BlogTypes.RemoveRange(_dbContext.BlogTypes.ToList());
                    _dbContext.SaveChanges();
                }
                foreach (var data in blogTypeList)
                {
                    var blogTypeExist = _dbContext.BlogTypes.Any(x => x.Title == data);
                    if (!blogTypeExist)
                    {
                        BlogType blogType = new BlogType();
                        blogType.Title = data;
                        _dbContext.BlogTypes.Add(blogType);
                    }
                }
                _dbContext.SaveChanges();
                return StatusCode(200, "Blog Type created successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
