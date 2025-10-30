using Newtonsoft.Json;
using CryptoLink.Domain.Aggregates.Users.Enums;
using CryptoLink.Domain.Aggregates.Users.ValueObjects;
using CryptoLink.Domain.Common.Primitives;
using CryptoLink.Domain.Aggregates.SessionGameLists;
using CryptoLink.Domain.DomainEvents;
using CryptoLink.Domain.Aggregates.Comments;


namespace CryptoLink.Domain.Aggregates.Users;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Password Password { get; set; }
    public DateTime Birthday { get; set; }
    
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? VerifiedOnUtc { get; private set; }
    public DateTime LastModifiedOnUtc { get; private set; }
    public VerificationToken VerificationToken { get; private set; }
    public ResetPasswordToken? ResetPasswordToken { get; private set; }
    public Ban? Ban { get; private set; }
    public Role Role { get; private set; }

    private readonly List<SessionGameList> _sessionGameList = new();
    public IReadOnlyCollection<SessionGameList> SessionGames => _sessionGameList.AsReadOnly();

    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();


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
        Password = Password.Create(password);
        Birthday = birthday;
        CreatedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        VerificationToken = VerificationToken.Create();
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
        var user = new User(
            default,
            firstName,
            lastName,
            email,
            password,
            birthday);
        
        user.RaiseDomainEvent(new UserRegistered(
            firstName,
            lastName,
            email,
            user.VerificationToken.Value,
            verifyEmailUrl));

        return user;
    }
    
    public User Load(
        UserId id,
        string firstName,
        string lastName,
        string email,
        Password password,
        DateTime birthday,
        DateTime createdOnUtc,
        DateTime? verifiedOnUtc,
        DateTime lastModifiedOnUtc,
        Ban ban,
        Role role,
        VerificationToken verificationToken,
        ResetPasswordToken resetPasswordToken
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Birthday = birthday;
        CreatedOnUtc = createdOnUtc;
        VerifiedOnUtc = verifiedOnUtc;
        LastModifiedOnUtc = lastModifiedOnUtc;
        Ban = ban;
        Role = role;
        VerificationToken = verificationToken;
        ResetPasswordToken = resetPasswordToken;
        return this;
    }
    
    public bool IsVerified =>
        VerifiedOnUtc is not null;
    
    public bool Verify(Guid token)
    {
        // Check if user is already verified
        if (IsVerified)
        {
            return false;
        }
        
        // Check if token is valid
        if (!VerificationToken.Verify(token))
        {
            return false;
        }
        
        VerifiedOnUtc = DateTime.UtcNow;
        LastModifiedOnUtc = DateTime.UtcNow;
        return true;
    }
    
    public void GenerateNewVerificationToken(string verifyEmailUrl)
    {
        VerificationToken = VerificationToken.Create();
        
        RaiseDomainEvent(new VerificationTokenRefreshed(
            FirstName,
            Email,
            VerificationToken.Value,
            verifyEmailUrl));
    }
    
    
}