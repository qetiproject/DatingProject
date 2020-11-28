﻿namespace DatingApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        //public DateTime DateOfBirth { get; set; }
        //public string Knows { get; set; }
        //public DateTime Created { get; set; } = DateTime.Now;
        //public DateTime LastActive { get; set; } = DateTime.Now;
        //public string Gender { get; set; }
        //public string Introduction { get; set; }
        //public string lookingFor { get; set; }
        //public string Interests { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        //public ICollection<Photo> Photos { get; set; }
        ////public ICollection<Userlike> LikedByUsers { get; set; }
        ////public ICollection<Userlike> LikedUsers { get; set; }
        //public ICollection<Message> MessagesSent { get; set; }
        //public ICollection<Message> MessagesRecieved { get; set; }
    }
}