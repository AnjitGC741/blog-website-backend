using BlogWebsite.Data;
using BlogWebsite.Models.FormInput;
using BlogWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public CommentController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }
        [HttpPost]
        [Route("create-comment")]
        public IActionResult Create(int BlogId, string comment)
        {
            try
            {
                int? ActiveUserId = _contextAccessor.HttpContext.Session.GetInt32("UserId");
                var ActiveUserName = _contextAccessor.HttpContext.Session.GetString("UserName");
                if (ActiveUserId == null)
                {
                    return StatusCode(500, "Active user not found");

                }
                var blogDetail = _dbContext.Blogs.Any(x=>x.Id == BlogId);
                if(!blogDetail) {
                    return StatusCode(500, "Blog doesnot exists");

                }
                var alreadyCommented = _dbContext.Comments.Any(x => x.UserName == ActiveUserName && x.BlogId == BlogId);
                if(alreadyCommented)
                {
                    return StatusCode(500, "Cannot comment");
                }
                var newComment = new Comment
                {
                    UserName = ActiveUserName,
                    BlogId = BlogId,
                    Title = comment,
                    CommentedDate = DateTime.Now
                };
                _dbContext.Comments.Add(newComment);
                _dbContext.SaveChanges();
                return StatusCode(200,"Commented succesfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpGet]
        [Route("comment/{Id}")]
        public IActionResult GetComment(int Id)
        {
            try
            {
                var commentList = _dbContext.Comments.Where(x => x.BlogId == Id);
                if (commentList.Any())
                {
                    return StatusCode(200, commentList);

                }
                return StatusCode(500, "Comment cannot be found");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }


}
