using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class UserService : CrudService<UserDto, User>, IUserService
    {
        private readonly IUserRepository UserRepository;
        public UserService(ICrudRepository<User> crudRepository, IMapper mapper, IUserRepository userRepository) : base(crudRepository, mapper)
        {
            UserRepository = userRepository;
        }
        
        public Result<UserDto> GetById(long userId) 
        {
            User user = UserRepository.GetById(userId);
            return MapToDto(user);
        }
      
    }
}
