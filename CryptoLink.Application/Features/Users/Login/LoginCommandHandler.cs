using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Application.Utils;
using CryptoLink.Domain.Common.Cache;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;

namespace CryptoLink.Application.Features.Users.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly ICryptoService _cryptoService;
        private readonly ICacheService _cache;


        public LoginCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            ICryptoService cryptoService,
            ICacheService cache
)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _cache = cache;
            _cryptoService = cryptoService; 
        }

        public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string cacheKey = $"reg_pending_{request.UserName}";

            var pendingReg = await _cache.GetAsync<PendingRegistrationModel>(cacheKey);

            if (pendingReg == null)
            {
                return Errors.User.SessionTokenExpired;
            }

            if (pendingReg.ChallengeToken != request.DecryptedToken.Trim())
            {
                return Errors.User.SessionTokenExpired;
            }

            var user = await _userRepository.GetByNameAsync(request.UserName, cancellationToken);



            await _cache.RemoveAsync(cacheKey);
            var token = _jwtProvider.GenerateToken(user);


            LoginResponse loginResponse = new LoginResponse
            {
                Id = user.Id.Value,
                Name = user.Name,
                Token = token.Token,
                TokenExpiresOnUtc = token.ExpiresOnUtc
            };


            return loginResponse;
        }
    }
}