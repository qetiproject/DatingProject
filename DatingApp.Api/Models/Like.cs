namespace DatingApp.Api.Models
{
    public class Like
    {
        public int LikerId { get; set; }
        public int LikeeId { get; set; }
        public User liker { get; set; }
        public User Likee { get; set; }
    }
}
