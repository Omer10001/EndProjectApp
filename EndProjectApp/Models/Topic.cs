using System;
using System.Collections.Generic;


namespace EndProjectApp.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Posts = new List<Post>();
            Reviews = new List<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AboutText { get; set; }

        public virtual List<Post> Posts { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
