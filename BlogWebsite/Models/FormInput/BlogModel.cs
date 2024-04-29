using static BlogWebsite.Models.EnumValues;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogWebsite.Models.FormInput
{
    public class BlogModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public int UserId { get; set; }
        public int BlogTypeId { get; set; }
    }
}
