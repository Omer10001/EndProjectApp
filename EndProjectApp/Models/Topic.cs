using System;
using System.Collections.Generic;
using EndProjectApp.Services;


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
        public string Image { get; set; }
        public string AboutText { get; set; }
        public string ImgSource
        {
            get
            {
                EndProjectAPIProxy proxy = EndProjectAPIProxy.CreateProxy();
                //Create a source with cache busting!
                Random r = new Random();
                string source = $"{proxy.GetBasePhotoUri()}/Topic{this.Id}.jpg?{r.Next()}";
                return source;
            }
        }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
