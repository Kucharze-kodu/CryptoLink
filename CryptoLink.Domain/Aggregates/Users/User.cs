using CryptoLink.Domain.Common.Primitives;
using CryptoLink.Domain.Aggregates.Users.Enums;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Aggregates.LinkExtendeds;

namespace CryptoLink.Domain.Aggregates.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string Name { get; set; }
    public string PublicKey { get; set; }

    public LinkExtended LinksExtended { get; set; } = null;


    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? VerifiedOnUtc { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }
    public Ban? Ban { get; private set; }
    public Role Role { get; private set; }


    private User(UserId id) : base(id)
    {
    }

    private User(
        UserId id,
        string name,
        string publicKey) : base(id)
    {
        Name = name;
        PublicKey = publicKey;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        Role = Role.User;
    }

    public static User Create(
        string name,
        string publicKey
    )
    {
        var User = new User(
            default,
            name,
            publicKey);

        return User;
    }
    
    public User Load(
        UserId id,
        string firstName,
        string lastName,
        string name,
        string publicKey,
        DateTime createdOnUtc,
        DateTime? verifiedOnUtc,
        DateTime lastModifiedOnUtc,
        Ban ban,
        Role role
    )
    {
        Id = id;
        Name = name;
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