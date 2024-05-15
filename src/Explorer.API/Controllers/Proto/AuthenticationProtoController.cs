
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Grpc.Core;
using GrpcServiceTranscoding;

public class AuthenticationProtoController : Authorize.AuthorizeBase
{
    private readonly ILogger<AuthenticationProtoController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationProtoController(ILogger<AuthenticationProtoController> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public override Task<AuthenticationTokens> Authorize(Credentials request,
        ServerCallContext context)
    {
        var credentials = new CredentialsDto { Password = request.Password, Username = request.Username };
        var result = _authenticationService.Login(credentials);

        return Task.FromResult(new AuthenticationTokens
        {
            Id = (int)result.Value.Id,
            AccessToken = result.Value.AccessToken,
        });
    }

    public override Task<AuthenticationTokens> Register(AccountRegistration request,
               ServerCallContext context)
    {
        var account = new AccountRegistrationDto
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
            Role = (UserRoleDto)request.Role,
            Surname = request.Surname,
            Username = request.Username
        };

        var result = _authenticationService.RegisterTourist(account);

        return Task.FromResult(new AuthenticationTokens
        {
            Id = (int)result.Value.Id,
            AccessToken = result.Value.AccessToken,
        });
    }
}
