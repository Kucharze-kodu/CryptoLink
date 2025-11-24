using Newtonsoft.Json;
using CryptoLink.Domain.Common.Primitives;
using CryptoLink.Domain.Aggregates.Users.Enums;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Aggregates.LinkExtendeds;

namespace CryptoLink.Domain.Aggregates.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public PublicKey PublicKey { get; set; }

    public LinkExtended LinksExtended { get; set; } = null;


    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? VerifiedOnUtc { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }
    public Ban? Ban { get; private set; }
    public Role Role { get; private set; }


    private User(UserId id) : base(id)
    {
    }

    [JsonConstructor]
    private User(
        UserId id,
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PublicKey = PublicKey.Create(password);
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        Role = Role.User;
    }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string password,
        DateTime birthday,
        string verifyEmailUrl
    )
    {
        var User = new User(
            default,
            firstName,
            lastName,
            email,
            password,
            birthday);

        return User;
    }
    
    public User Load(
        UserId id,
        string firstName,
        string lastName,
        string email,
        PublicKey publicKey,
        DateTime createdOnUtc,
        DateTime? verifiedOnUtc,
        DateTime lastModifiedOnUtc,
        Ban ban,
        Role role
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PublicKey = publicKey;
        CreatedOnUtc = createdOnUtc;
        VerifiedOnUtc = verifiedOnUtc;
        LastModifiedOnUtc = lastModifiedOnUtc;
        Ban = ban;
        Role = role;
        return this;
    }
    
    public bool IsVerified =>
        VerifiedOnUtc is not null;
    
    public bool Verify(Guid token)
    {
        // Check if User is already verified
        if (IsVerified)
        {
            return false;
        }
        
        VerifiedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        return true;
    }
    
    
}