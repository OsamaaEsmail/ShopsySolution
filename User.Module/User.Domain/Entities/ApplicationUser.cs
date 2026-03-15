

using Microsoft.AspNetCore.Identity;

namespace User.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{



    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public bool IsDisabled { get; set; }


    public List<RefreshToken> RefreshTokens { get; set; } = []; //One To Meny "User (One) Have RefreshTokens (Meny) "




}