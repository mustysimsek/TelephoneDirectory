using System;
using System.Threading.Tasks;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.PersonContact.Service.Services.Abstracts
{
	public interface IPersonService
	{
        Task<List<Person>> GetAllAsync();
        Task<Response<Person>> CreateAsync(PersonDto personDto);
        Task<Response<List<PersonContactInfoDto>>> GetbyIdAsync(Guid personUuid);
        Task<Response<NoContent>> DeleteAsync(Guid personUuid);
    }
}

