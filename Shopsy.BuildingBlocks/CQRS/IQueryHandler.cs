



using MediatR;
using Shopsy.BuildingBlocks.Abstractions;

namespace Shopsy.BuildingBlocks.CQRS;


public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }

