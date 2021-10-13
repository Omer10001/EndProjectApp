using System;
using System.Collections.Generic;



namespace EndProjectApp.Models
{
    public partial class TagsInPost
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int PostId { get; set; }

        public virtual Post Post { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
