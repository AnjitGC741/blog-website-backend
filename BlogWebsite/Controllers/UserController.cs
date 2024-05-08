using BlogWebsite.Data;
using BlogWebsite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public UserController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }
        [HttpGet]
        [Route("my-profile")]
        public IActionResult MyProfile()
        {
            try
            {
                int? userId = _contextAccessor.HttpContext.Session.GetInt32("UserId");
                if(userId == null)
                {
                    return StatusCode(500, "Active user not found");

                }
                var user = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
                return StatusCode(200, user);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpPut]
        [Route("edit-profile")]
        public IActionResult EditProfile(string Name, string Email, string Address, string Contact)
        {
            try
            {
                int? userId = _contextAccessor.HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return StatusCode(500, "Active user not found");

                }
                var userDetail = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userDetail == null)
                {
                    return StatusCode(500, "User not found");
                }
                userDetail.Name = Name;
                userDetail.Email = Email;
                userDetail.Address = Address;   
                userDetail.Contact = Contact;
                _dbContext.Users.Update(userDetail);
                _dbContext.SaveChanges();
                return StatusCode(200, "Account updated successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("delete-profile")]
        public IActionResult DeleteProfile(string Name, string Email, string Address, string Contact)
        {
            try
            {
                int? userId = _contextAccessor.HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    return StatusCode(500, "Active user not found");

                }
                var userDetail = _dbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userDetail == null)
                {
                    return StatusCode(500, "User not found");
                }
                _dbContext.Users.Remove(userDetail);
                _dbContext.SaveChanges();
                return StatusCode(200, "Account Deleted successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
}
