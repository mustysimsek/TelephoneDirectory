using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Core.Enums;

namespace TelephoneDirectory.PersonContact.Service.Services.Dtos
{
	public class PersonContactInfoDto
	{

        public PersonDto Person { get; set; }
        public Guid PersonId { get; set; }
        public PersonContactInfoType PersonContactInfoType { get; set; }
        public string Content { get; set; }
    }
}

