﻿namespace PeaLearning.Api.Requests.Users
{
    public class UserPasswordChangeRequest
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}