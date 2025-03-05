// Post.cs
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public enum Category
    {
        Humour,
        Nouvelle,
        Inconnue
    }
    public class PostForm: Post
    {
        public required IFormFile FileToUpload { get; set; }

    }
    public class Post
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public byte[]? Image { get; set; }

        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;

        //[Display(Name = "Contenue revisé ?")]
        //public bool IsApproved { get; private set; } = false;

        //public bool IsDeleted { get; private set; } = false;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public List<Comment> Comments { get; set; } = new();

        public void IncrementLike()
        {
            Like++;
        }

        public void IncrementDislike()
        {
            Dislike++;
        }

        //push this to keep track
        //public void Approve()
        //{
        //    IsApproved = true;
        //}

        //public void Delete()
        //{
        //    IsDeleted = true;
        //}

    }
}
