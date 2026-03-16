using Shopsy.BuildingBlocks.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.DtoContracts;

namespace User.Application.Interfaces;

public interface IRoleService
{
    Task<Result<IEnumerable<RoleResponse>>> GetAllAsync();

    Task<Result<RoleDetailResponse>> GetByIdAsync(string roleId);

    Task<Result> CreateAsync(string name, IEnumerable<string> permissions);

    Task<Result> UpdateAsync(string roleId, string name, IEnumerable<string> permissions);
}