using AutoMapper;
using KitchenDeliverySystem.CrossCutting.Utility;
using KitchenDeliverySystem.Domain.Repositories;
using KitchenDeliverySystem.Domain.UnitOfWork;
using KitchenDeliverySystem.Dto.User;

namespace KitchenDeliverySystem.Application.UseCases.User.UserInsert
{
    public class UserCreateUseCase : IUserCreateUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserCreateUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = (IUserRepository)_unitOfWork.Repository<Domain.Entities.User>();
        }

        public async Task<UserDto> ExecuteAsync(CreateUserDto inbound)
        {
            var passwordHasDto = PasswordUtility.CreatePasswordHash(inbound.Password);

            var user = new Domain.Entities.User(
                true,
                inbound.FirstName,
                inbound.LastName,
                inbound.UserName,
                passwordHasDto.PasswordHash,
                passwordHasDto.PasswordSalt);

            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }
    }
}
