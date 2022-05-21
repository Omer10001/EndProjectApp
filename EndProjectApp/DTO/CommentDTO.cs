using System;
using System.Collections.Generic;
using System.Text;
using EndProjectApp.Models;
using Xamarin.Forms;

namespace EndProjectApp.DTO
{
    class CommentDTO
    {
        public Comment Comment { get; set; }
        public LikesInComment LikeInComment { get; set; }
        public string LikedIconFont
        {
            get
            {
                if (LikeInComment.IsLiked)
                    return "FAS";
                else
                    return "FAR";
            }
        }
        public string DisLikedIconFont
        {
            get
            {
                if (LikeInComment.IsDisliked)
                    return "FAS";
                else
                    return "FAR";
            }
        }
    }
}
