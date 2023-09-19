using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.ControllerBases;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelephoneDirectory.PersonContact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonContactInfoController : CustomBaseController
    {
        private readonly IPersonContactInfoService _personContactInfoService;

        public PersonContactInfoController(IPersonContactInfoService personContactInfoService)
        {
            _personContactInfoService = personContactInfoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonContactInfoDto request)
        {
            var response = await _personContactInfoService.CreateAsync(request);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _personContactInfoService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}

