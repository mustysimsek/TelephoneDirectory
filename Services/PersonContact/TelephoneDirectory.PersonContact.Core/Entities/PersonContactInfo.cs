using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using TelephoneDirectory.PersonContact.Core.Common;
using TelephoneDirectory.PersonContact.Core.Enums;

namespace TelephoneDirectory.PersonContact.Core.Entities
{
	public class PersonContactInfo : Document
	{
        [BsonRepresentation(BsonType.ObjectId)]
        public string PersonId { get; set; }
        [BsonIgnore]
        public Person Person { get; set; }
        public PersonContactInfoType PersonContactInfoType { get; set; }
        public string Content { get; set; }
    }
}

