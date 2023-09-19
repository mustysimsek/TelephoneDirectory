using System;
using TelephoneDirectory.PersonContact.Core.Entities;

namespace TelephoneDirectory.PersonContact.Service.Services.Dtos
{
    public class PersonDetailsDto
    {
        public string Id { get; set; }
        public PersonDto PersonDto { get; set; }
        public PersonContactInfo PersonContactInfo { get; set; }
    }
}

