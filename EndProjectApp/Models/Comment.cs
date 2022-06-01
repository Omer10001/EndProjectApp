using System;
using System.Collections.Generic;



namespace EndProjectApp.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseRepliedTo = new List<Comment>();
            LikesInComments = new List<LikesInComment>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public int? RepliedToId { get; set; }
        public int UserId { get; set; }
        public int NumOfLikes { get; set; }
        public string Text { get; set; }
        public DateTime TimeCreated { get; set; }
        public string TimeSpanString { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment RepliedTo { get; set; }
        public virtual User User { get; set; }
        public virtual List<Comment> InverseRepliedTo { get; set; }
        public virtual List<LikesInComment> LikesInComments { get; set; }
    }
}
