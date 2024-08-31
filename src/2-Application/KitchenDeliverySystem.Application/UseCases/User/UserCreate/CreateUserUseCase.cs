using AutoMapper;
using ErrorOr;
using KitchenDeliverySystem.CrossCutting.ErrorCatalog;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserInsert
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CreateUserUseCase(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserDto>> ExecuteAsync(CreateUserDto inbound)
        {
            var userInDatabase = await _userRepository.GetByUsernameAsync(inbound.UserName);
            if (userInDatabase != null)
                return ErrorCatalog.UserAlterdyExists;

            var passwordHashDto = PasswordUtility.CreatePasswordHash(inbound.Password);

            var user = new Domain.Entities.User(
                true,
                inbound.FirstName,
                inbound.LastName,
                inbound.UserName,
                passwordHashDto.PasswordHash,
                passwordHashDto.PasswordSalt);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
