

using Microsoft.AspNetCore.Http;
using Shopsy.BuildingBlocks.Abstractions;

namespace User.Domain.Errors;


public record class UserErrors
{
    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email/Password", StatusCodes.Status401Unauthorized);

    public static readonly Error DisabledUser =
    new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

    public static readonly Error LockedUser =
      new("User.LockedUser", "LockedUser ,please contact your administrator", StatusCodes.Status401Unauthorized);



    public static readonly Error InvalidJwtToken =
       new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error EmailAlreadyExists =
        new("User.EmailAlreadyExists", "Email already exists", StatusCodes.Status409Conflict);



    public static readonly Error DuplicatedEmail =
        new("User.DuplicatedEmails", "Email already exists", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
       new("User.EmailNotConfirmed", "Email Not Confirmed", StatusCodes.Status400BadRequest);


    public static readonly Error InvalidCode =
     new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);


    public static readonly Error DuplicatedConfirmation =
     new("User.DuplicatedConfirmation", "Email already Confirmed", StatusCodes.Status400BadRequest);

    public static readonly Error UserNotFound =
     new("User.UserNotFound", "User is not Found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidRoles =
      new("User.InvalidRoles", "Invalid roles", StatusCodes.Status400BadRequest);


}

