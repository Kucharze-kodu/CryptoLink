using CryptoLink.Application.Common.Messaging;
using CryptoLink.Application.Contracts.Users;
using CryptoLink.Application.Persistance;
using CryptoLink.Application.Persistance.CryptoLink.Application.Common.Interfaces;
using CryptoLink.Domain.Aggregates.Users;
using CryptoLink.Domain.Common.Cache;
using CryptoLink.Domain.Common.Errors;
using ErrorOr;



namespace CryptoLink.Application.Features.Users.Register
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cache;
        private readonly ICryptoService _cryptoService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            ICacheService cache,
            ICryptoService cryptoService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _cache = cache;
            _cryptoService = cryptoService;
            _unitOfWork=unitOfWork;
        }

        public async Task<ErrorOr<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
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


            if (await _userRepository.AnyUserAsync(request.UserName))
            {
                return Errors.User.DuplicateName;
            }

            var newUser = User.Create(
                    request.UserName,
                    pendingReg.PublicKey
                );


            await _userRepository.AddUserAsync(newUser);
            await _cache.RemoveAsync(cacheKey);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return new RegisterResponse("User registered successfully.");
        }
    }
}

        