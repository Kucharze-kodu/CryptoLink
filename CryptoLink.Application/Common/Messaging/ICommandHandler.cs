using ErrorOr;
using MediatR;

namespace CryptoLink.Application.Common.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ErrorOr<Unit>>
    where TCommand : ICommand
{
    
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ErrorOr<TResponse>>
    where TCommand : ICommand<TResponse>
{
    
}