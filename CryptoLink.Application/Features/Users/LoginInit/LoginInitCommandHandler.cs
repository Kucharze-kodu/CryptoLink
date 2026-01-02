using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Common.Cache;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.Users.LoginInit
{
    public class LoginInitCommandHandler : ICommandHandler<LoginInitCommand, LoginInitResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ICryptoService _cryptoService;
        private readonly ICacheService _cache;


        public LoginInitCommandHandler(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            ICryptoService cryptoService,
            ICacheService cache)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _cryptoService = cryptoService;
            _cache = cache;
        }


        public async Task<ErrorOr<LoginInitResponse>> Handle(LoginInitCommand request, CancellationToken cancellationToken)
        {

            User? user = await _userRepository.GetByNameAsync(request.UserName, cancellationToken);

            if (user is null)
            {
                return Errors.User.NotFound;
            }    

            var (token, encryptedMessage) = await _cryptoService.GenerateChallengeAsync(user.PublicKey);

            var pendingRegistration = new PendingRegistrationModel
            {
                PublicKey = user.PublicKey,
                ChallengeToken = token
            };

            encryptedMessage = string.Join(Environment.NewLine,
                encryptedMessage
                    .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                    .Where(line =>
                        !line.StartsWith("Version:"))
                    .Select(line => line.Trim())
            );

            await _cache.SetAsync(
                    $"reg_pending_{request.UserName}",
                    pendingRegistration,
                    TimeSpan.FromMinutes(10)); // Set 10-minute expiration

            return new LoginInitResponse(encryptedMessage);
        }
    }
}
