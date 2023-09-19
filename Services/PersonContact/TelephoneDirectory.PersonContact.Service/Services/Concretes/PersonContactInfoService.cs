using System;
using Mapster;
using MongoDB.Driver;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Repository.Configurations;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.PersonContact.Service.Services.Concretes
{
	public class PersonContactInfoService : IPersonContactInfoService
    {
        private readonly IMongoCollection<PersonContactInfo> _personContactInfoCollection;
        private readonly IMongoCollection<Person> _personCollection;

        public PersonContactInfoService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _personContactInfoCollection = database.GetCollection<PersonContactInfo>
                (databaseSettings.PersonContactInfoCollectionName);
            _personCollection = database.GetCollection<Person>(databaseSettings.PersonCollectionName);
        }

        public async Task<Response<PersonContactInfo>> CreateAsync(PersonContactInfoDto personContactInfoDto)
        {
            var newPersonContactInfo = personContactInfoDto.Adapt<PersonContactInfo>();
            newPersonContactInfo.CreateDate = DateTime.Now;

            await _personContactInfoCollection.InsertOneAsync(newPersonContactInfo);

            return Response<PersonContactInfo>.Success(personContactInfoDto.Adapt<PersonContactInfo>(), 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(Guid personContactInfoUuid)
        {
            var result = await _personContactInfoCollection.DeleteOneAsync(x => x.UUID == personContactInfoUuid);

            if (result.DeletedCount>0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("There is no Person Contact Info with this ID", 404);
            }
        }
    }
    
}

