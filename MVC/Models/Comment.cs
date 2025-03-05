// Comment.cs
using Newtonsoft.Json;

namespace MVC.Models
{
    //comment class
    public class Comment
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string PostId { get; set; } = string.Empty;
        public string Commentaire { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;


        public int Like { get; set; } = 0;
        public int Dislike { get; set; } = 0;
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public void IncrementLike()
        {
            Like++;
        }

        public void IncrementDislike()
        {
            Dislike++;
        }
    }
}
