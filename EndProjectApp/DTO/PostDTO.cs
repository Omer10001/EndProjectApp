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
        public string LikedIconFont
        {
            get
            {
                if (LikeInPost.IsLiked)
                    return "FAS";
                else
                    return "FAR";
            }
        }
        public string DisLikedIconFont
        {
            get
            {
                if (LikeInPost.IsDisliked)
                    return "FAS";
                else
                    return "FAR";
            }
        }

        public PostDTO()
        {
            
        }

    }
}
