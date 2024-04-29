using BlogWebsite.Data;
using BlogWebsite.Models;
using BlogWebsite.Models.FormInput;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsite.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;
        public AuthController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext) { 
           _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password) {
            try
            {

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
            return Ok();
        }

        [HttpPost]
        [Route("register-user")]
        public IActionResult RegisterUser([FromForm] RegisterUserModel user)
        {
            try
            {
                User createUser = new User();
                createUser.Name = $"{user.FirstName} {user.LastName}";
                createUser.Address = user.Address;
                createUser.Email = user.Email;
                createUser.Password = user.Password;
                createUser.Contact = user.Contact;
                createUser.UserRole = user.UserRole;
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (user.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.Image.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Image");
                    string filePath = Path.Combine(productPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        user.Image.CopyTo(fileStream);
                    }

                    createUser.ImagePath = @"/Image/" + fileName;
                }
                _dbContext.Users.Add(createUser);
                _dbContext.SaveChanges();
                return StatusCode(200, "User Created Successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

    }
}

//return BadRequest("Invalid PDF file");
