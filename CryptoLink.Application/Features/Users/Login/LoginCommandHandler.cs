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

            return new LoginResponse(
                user.Id.Value,
                user.Name,
                token.Token,
                token.ExpiresOnUtc);

        }
    }
}


/*
 * 
public class InitiateLoginCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly ICryptoService _cryptoService;
    private readonly ICacheService _cache; // Abstrakcja do cache'a (Redis/Memory)

    public async Task<string> Handle(InitiateLoginCommand command)
    {
        // 1. Pobierz użytkownika z bazy
        var user = await _userRepository.GetByUsernameAsync(command.Username);
        if (user == null) throw new NotFoundException("User not found");

        // 2. Wygeneruj challenge używając PGP
        var (token, encryptedMessage) = await _cryptoService.GenerateChallengeAsync(user.PublicKey);

        // 3. Zapisz "czysty" token w cache powiązany z tym użytkownikiem
        // Klucz w cache: "auth_challenge_{username}", Wartość: "token_guid"
        await _cache.SetAsync($"auth_challenge_{user.Username}", token, TimeSpan.FromMinutes(2));

        // 4. Zwróć zaszyfrowaną wiadomość do klienta
        // Klient musi ją odszyfrować swoim kluczem prywatnym i odesłać treść.
        return encryptedMessage; 
    }
}

*/