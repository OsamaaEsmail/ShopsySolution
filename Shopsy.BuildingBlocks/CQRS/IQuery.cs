



using MediatR;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.BuildingBlocks.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
