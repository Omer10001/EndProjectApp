﻿using System;
using System.Collections.Generic;


namespace EndProjectApp.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new List<Comment>();
            Posts = new List<Post>();
            Reviews = new List<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual List<Comment> Comments { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
