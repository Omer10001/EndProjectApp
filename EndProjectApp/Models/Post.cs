using System;
using System.Collections.Generic;
using System.Text;
using EndProjectApp.Services;



namespace EndProjectApp.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
           
            LikesInPosts = new List<LikesInPost>();
        }

        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int NumOfLikes { get; set; }
        public string Text { get; set; }
      
        public string Title { get; set; }
        public DateTime TimeCreated { get; set; }
        public string TimeSpanString { get; set; }
        public string ImgSource
        {
            get
            {
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                //Create a source with cache busting!
                Random r = new Random();
                string source = $"{proxy.GetBasePhotoUri()}/Post{this.Id}.jpg?{r.Next()}";
                return source;
            }
        }

        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
        public virtual List<LikesInPost> LikesInPosts { get; set; }
        public virtual List<Comment> Comments { get; set; }
      
    }
}
