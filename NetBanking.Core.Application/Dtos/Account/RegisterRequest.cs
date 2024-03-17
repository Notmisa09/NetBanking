﻿namespace NetBanking.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string IdCard { get; set; }
        public bool UserStatus { get; set; }
        public string PhoneNumber { get; set; }
    }
}
