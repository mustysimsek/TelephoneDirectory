using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using TelephoneDirectory.PersonContact.Core.Common;

namespace TelephoneDirectory.PersonContact.Core.Entities
{
	public class Person : Document
	{
        public string Name { get; set; } = null!;
        public string? Surname { get; set; }
        public string? Company { get; set; }
        [BsonElement("personContactInfos")]
        public List<PersonContactInfo> PersonContactInfos { get; set; }
    }
}

