



namespace User.Application.DtoContracts;

public record class UserResponse
(

    string Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);
