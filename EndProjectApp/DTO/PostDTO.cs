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
        public Color LikeColor {
            get
            {
                if (LikeInPost.IsLiked)
                {
                    return Color.Red;
                }
                else
                    return Color.Gray;
            }
            set
            {
                LikeColor = value;
            }
        }
        public Color DislikeColor {
            get
            {
                if (LikeInPost.IsDisliked)
                {
                    return Color.Blue;
                }
                else
                    return Color.Gray;
            }
            set
            {
                DislikeColor = value;
            }
        }


        public PostDTO()
        {
            
        }

    }
}
