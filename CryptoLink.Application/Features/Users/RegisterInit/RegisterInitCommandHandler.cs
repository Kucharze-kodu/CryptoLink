using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Domain.Common.Cache;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;
using System.Text.RegularExpressions;

namespace CryptoLink.Application.Features.Users.RegisterInit
{
    public class RegisterInitCommandHandler : ICommandHandler<RegisterInitCommand, RegisterInitResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly ICacheService _cache;

        public RegisterInitCommandHandler(
            IUserRepository userRepository,
            ICryptoService cryptoService,
            ICacheService cache)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _cache = cache;
        }

        public async Task<ErrorOr<RegisterInitResponse>> Handle(RegisterInitCommand request, CancellationToken cancellationToken)
        {
            if (!_cryptoService.ValidatePublicKey(request.PublicKey))
            {
                return Errors.User.InvalidTokenPGP;
            }

            var rawUser = _cryptoService.ExtractUserIdFromPublicKey(request.PublicKey);

            string name = "Unknown";

            if (!string.IsNullOrEmpty(rawUser))
            {
                var match = Regex.Match(rawUser, @"^(.*?)<([^>]+)>$");

                if (match.Success)
                {
                    name = match.Groups[1].Value.Trim();
                }
                else
                {
                    return Errors.User.InvalidTokenPGP;
                }
            }

            if (await _userRepository.AnyUserAsync(name, cancellationToken))
            {
                return Errors.User.DuplicateName;
            }

            var (token, encryptedMessage) = await _cryptoService.GenerateChallengeAsync(request.PublicKey);

            var pendingRegistration = new PendingRegistrationModel
            {
                PublicKey = request.PublicKey,
                ChallengeToken = token
            };

            await _cache.SetAsync(
                $"reg_pending_{request.UserName}",
                pendingRegistration,
                TimeSpan.FromMinutes(10)); // Dajemy 10 min na dokończenie rejestracji

            // 5. Zwracamy zaszyfrowaną wiadomość
            return new RegisterInitResponse(encryptedMessage);
        }
    }
}

