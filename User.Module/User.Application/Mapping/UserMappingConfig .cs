
using Mapster;
using User.Application.DtoContracts;
using User.Domain.Entities;

namespace User.Application.Mapping;

public class UserMappingConfig : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ApplicationUser, UserResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Email, src => src.Email!)
            .Map(dest => dest.IsDisabled, src => src.IsDisabled)
            .Ignore(dest => dest.Roles);

        config.NewConfig<ApplicationUser, UserProfileResponse>()
            .Map(dest => dest.Email, src => src.Email!)
            .Map(dest => dest.UserName, src => src.UserName!);

        config.NewConfig<ApplicationUser, AuthResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Ignore(dest => dest.Token)
            .Ignore(dest => dest.ExpiresIn)
            .Ignore(dest => dest.RefreshToken)
            .Ignore(dest => dest.RefreshTokenExpiryDate);
    }

}
