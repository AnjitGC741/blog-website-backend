using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BlogWebsite.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImagePath { get; set; }
        public DateTime WrittenDate { get; set; }
        public int? UpVote {  get; set; }
        public int? DownVote { get; set;}
        public bool IsEdited { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("BlogType")]
        public int BlogTypeId { get; set; }
        public BlogType BlogType { get; set; }
    }
}
