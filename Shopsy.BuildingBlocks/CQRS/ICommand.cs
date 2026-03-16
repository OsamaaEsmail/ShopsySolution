


using MediatR;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.BuildingBlocks.CQRS;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

