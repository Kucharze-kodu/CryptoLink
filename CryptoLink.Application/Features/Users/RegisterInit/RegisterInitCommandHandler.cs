using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Domain.Common.Cache;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;
using System.Linq;
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

            var key = request.PublicKey
                .Replace("-----BEGIN PGP PUBLIC KEY BLOCK-----", "")
                .Replace("-----END PGP PUBLIC KEY BLOCK-----", "")
                .Split('\n')
                .Where(line =>
                    !line.StartsWith("Comment:") &&
                    !line.StartsWith("="))
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line));

            var cleanedKey = string.Concat(key);

            var rawUser = _cryptoService.ExtractUserIdFromPublicKey(cleanedKey);
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

            if (!_cryptoService.ValidatePublicKey(cleanedKey))
            {
                return Errors.User.InvalidTokenPGP;
            }


            if (await _userRepository.AnyUserAsync(name, cancellationToken))
            {
                return Errors.User.DuplicateName;
            }

            var (token, encryptedMessage) = await _cryptoService.GenerateChallengeAsync(cleanedKey);

            var pendingRegistration = new PendingRegistrationModel
            {
                PublicKey = cleanedKey,
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
                TimeSpan.FromMinutes(10)); // Expires in 10 minutes

            return new RegisterInitResponse(encryptedMessage);
        }
    }
}

