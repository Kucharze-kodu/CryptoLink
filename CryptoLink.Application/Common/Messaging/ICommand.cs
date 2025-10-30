using ErrorOr;
using MediatR;

namespace CryptoLink.Application.Common.Messaging;

public interface ICommand : IRequest<ErrorOr<Unit>>
{
    
}

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}