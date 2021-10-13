using System;
using System.Collections.Generic;



namespace EndProjectApp.Models
{
    public partial class Tag
    {
        public Tag()
        {
            TagsInPosts = new List<TagsInPost>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public string Name { get; set; }

        public virtual List<TagsInPost> TagsInPosts { get; set; }
    }
}
