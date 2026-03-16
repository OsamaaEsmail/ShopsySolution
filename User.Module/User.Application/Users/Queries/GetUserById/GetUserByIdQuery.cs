using Shopsy.BuildingBlocks.CQRS;
using User.Application.DtoContracts;

namespace User.Application.Users.Queries.GetUserById;


public record GetUserByIdQuery(string UserId) : IQuery<UserResponse>;