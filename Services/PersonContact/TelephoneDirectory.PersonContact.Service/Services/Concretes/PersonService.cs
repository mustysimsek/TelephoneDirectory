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
    public class PersonService : IPersonService
    {
        private readonly IMongoCollection<Person> _personCollection;
        private readonly IMongoCollection<PersonContactInfo> _personContactInfoCollection;

        public PersonService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _personCollection = database.GetCollection<Person>(databaseSettings.PersonCollectionName);
            _personContactInfoCollection = database.GetCollection<PersonContactInfo>
                (databaseSettings.PersonContactInfoCollectionName);
        }

        public async Task<Response<List<Person>>> GetAllAsync()
        {
            var persons = await _personCollection.Find(person => true).ToListAsync();

            return Response<List<Person>>.Success(persons, 200);
        }
        public async Task<Response<Person>> CreateAsync(PersonDto personDto)
        {
            var entity = personDto.Adapt<Person>();
            entity.CreateDate = DateTime.Now;
            if (entity == null)
            {
                return Response<Person>.Fail("There are missing values", 404);
            }
            await _personCollection.InsertOneAsync(entity);
            return Response<Person>.Success(personDto.Adapt<Person>(), 200);
        }

        public async Task<Response<Person>> GetbyIdAsync(string personUuid)
        {
            var person = await _personCollection.Find<Person>(x => x.UUID == personUuid).FirstOrDefaultAsync();

            if (person == null)
            {
                return Response<Person>.Fail("Person not found", 404);
            }

            var hasPersonContactInfo = await _personContactInfoCollection.
                Find<PersonContactInfo>(i => i.PersonId == personUuid).ToListAsync();

            person.PersonContactInfos = hasPersonContactInfo;

            return Response<Person>.Success(person, 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(string personUuid)
        {
            var result = await _personCollection.DeleteOneAsync(x => x.UUID == personUuid);

            if (result.DeletedCount > 0)
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

