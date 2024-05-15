using AutoMapper;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Net.Http.Headers;
using System.Net.WebSockets;

namespace Explorer.API.Controllers
{
    //[Authorize(Policy = "touristAndAuthorPolicy")]
    [Route("api/person")]
    public class PersonController : BaseApiController
    {
        private readonly IPersonService _personService;


        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        //[Authorize(Policy = "touristAndAdminPolicy")]
        [HttpGet("{id:int}")]
        public ActionResult<UpdatePersonDTO> Get(int id) 
        {
            var result = _personService.GetByUserId(id);
            return CreateResponse(result);
        }
    }
}
