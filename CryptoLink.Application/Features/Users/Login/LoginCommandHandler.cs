using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLink.Application.Features.Users.Login
{
    internal class LoginCommandHandler
    {



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