using Explorer.Stakeholders.API.Public;
using Grpc.Core;
using GrpcServiceTranscoding;

namespace Explorer.API.Controllers.Proto
{
    public class PersonProtoController : PersonService.PersonServiceBase
    {
        private readonly IPersonService _personService;

        public PersonProtoController(IPersonService personService)
        {
            _personService = personService;
        }

        public override Task<PersonDto> GetByUserId(PersonIdRequest request, ServerCallContext context)
        {
            var person = _personService.GetByUserId(request.UserId).Value;
            var personDto = new PersonDto
            {
                Id = person.Id,
                UserId = person.UserId,
                Name = person.Name,
                Surname = person.Surname,
                Email = person.Email,
                Motto = person.Motto,
                Biography = person.Biography,
                Image = person.Image,
                Latitude = decimal.ToDouble(person.Latitude),
                Longitude = decimal.ToDouble(person.Longitude),
            };

            return Task.FromResult(personDto);
        }
    }
}
