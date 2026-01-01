using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;
using System.Text.RegularExpressions;



namespace CryptoLink.Application.Features.Users.Register
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;

        public RegisterUserCommandHandler(IUserRepository userRepository, ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
        }


        public async Task<ErrorOr<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
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


            // 3. Zapis użytkownika z kluczem publicznym
            User user = User.Create(
                name,
                request.PublicKey
            );

            await _userRepository.AddUserAsync(user, cancellationToken);

            return new RegisterResponse("You have register to us");
        }
    }
}
