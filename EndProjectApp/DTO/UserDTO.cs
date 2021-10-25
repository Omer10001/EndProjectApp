using System;
using System.Collections.Generic;
using System.Text;

namespace EndProjectApp.DTO
{
    class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime BirthDate { get; set; }
        //public UserDTO(string name, string email, string password, DateTime birth);
    }
}
