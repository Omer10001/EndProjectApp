using System;
using System.Collections.Generic;
using System.Text;
using EndProjectApp.Models;
using Xamarin.Forms;

namespace EndProjectApp.DTO
{
    class PostDTO
    {
        public Post Post {get; set;}
        public LikesInPost LikeInPost { get; set; }
        public Color LikeColor { get; set; }
        public Color DislikeColor { get; set; }


        public PostDTO()
        {
            
        }

    }
}
