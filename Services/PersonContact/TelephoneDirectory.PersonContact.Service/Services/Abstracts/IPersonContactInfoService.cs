using System;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.PersonContact.Service.Services.Abstracts
{
	public interface IPersonContactInfoService
	{
        Task<Response<PersonContactInfo>> CreateAsync(PersonContactInfoDto personContactInfoDto);
        Task<Response<NoContent>> DeleteAsync(string personContactInfoUuid);
    }
}

