using System;
using Mapster;
using MongoDB.Driver;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Repository.Configurations;
using TelephoneDirectory.PersonContact.Repository.Repositories;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.PersonContact.Service.Services.Concretes
{
    public class PersonService : IPersonService
    {
        private readonly IMongoCollection<Person> _personCollection;
        private readonly IMongoCollection<PersonContactInfo> _personContactInfoCollection;

        public PersonService(IPersonRepository personRepository,
            IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _personCollection = database.GetCollection<Person>(databaseSettings.PersonCollectionName);
            _personContactInfoCollection = database.GetCollection<PersonContactInfo>
                (databaseSettings.PersonContactInfoCollectionName);
        }

        public async Task<List<Person>> GetAllAsync()
        {
            var persons = await _personCollection.Find(person => true).ToListAsync();

            return persons;
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

        public async Task<Response<List<PersonContactInfoDto>>> GetbyIdAsync(Guid personUuid)
        {
            var personContactInfos = await _personContactInfoCollection.
                Find<PersonContactInfo>(x => x.PersonId == personUuid).ToListAsync();

            if (personContactInfos == null)
                //Response<List<PersonContactInfoDto>>.Fail("PersonContactInfo not found", 404);
                personContactInfos = new List<PersonContactInfo>();

            foreach (var item in personContactInfos)
            {
                item.Person = await _personCollection.
                    Find<Person>(x => x.UUID == item.PersonId).FirstAsync();
            }
            return Response<List<PersonContactInfoDto>>.
                Success(personContactInfos.Adapt<List<PersonContactInfoDto>>(), 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(Guid personUuid)
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

