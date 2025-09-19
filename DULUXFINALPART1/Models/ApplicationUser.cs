using Microsoft.AspNetCore.Identity;
using System;

public class ApplicationUser : IdentityUser
{
    public DateTime? LastLoginTime { get; set; } // Tracks last login

    public DateTime? LastLogoutTime { get; set; }
}
