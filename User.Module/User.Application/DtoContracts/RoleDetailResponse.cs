



namespace User.Application.DtoContracts;


public record RoleDetailResponse(
    string Id, string Name, bool IsDeleted,
    IEnumerable<string> Permissions);