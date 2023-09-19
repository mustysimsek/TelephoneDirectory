using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelephoneDirectory.PersonContact.Core.Common
{
    public class Document : IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid UUID { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreateDate { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModifiedDate { get; set; }
    }
}

