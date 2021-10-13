using System;
using System.Collections.Generic;



namespace EndProjectApp.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new List<Comment>();
            TagsInPosts = new List<TagsInPost>();
        }

        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public int NumOfLikes { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public DateTime TimeCreated { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<TagsInPost> TagsInPosts { get; set; }
    }
}
