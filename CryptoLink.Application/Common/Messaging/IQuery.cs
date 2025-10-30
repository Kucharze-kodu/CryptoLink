using ErrorOr;
using MediatR;

namespace CryptoLink.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>
{
    
}