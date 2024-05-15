using Explorer.Stakeholders.API.Public;
using Grpc.Core;
using GrpcServiceTranscoding;

namespace Explorer.API.Controllers.Proto
{
    public class UserProtoController : UserService.UserServiceBase
    {
        private readonly IUserService _userService;

        public UserProtoController(IUserService userService)
        {
            _userService = userService;
        }

        public override Task<UserDto> GetById(UserIdRequest request, ServerCallContext context)
        {
            var user = _userService.GetById(request.UserId).Value;
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = (UserRole)user.Role,
                IsActive = user.IsActive
            };

            return Task.FromResult(userDto);
        }
    }
}
