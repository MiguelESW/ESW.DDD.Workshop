using System;

namespace ESW.DDD.Workshop.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegisteredDate { get; set; }
        public UserType Type { get; set; }
        public bool IsActive { get; set; }
    }
    
    public enum UserType
    {
        Candidate,
        Interviewer
    }
}