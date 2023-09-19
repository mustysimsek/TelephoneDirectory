using System;
using MongoDB.Driver;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Repository.Configurations;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.PersonContact.Repository.Repositories
{
    public interface IPersonRepository
    {
        Task CreateAsync(Person person);
        Task<List<Person>> GetAllAsync();
    }

    public class PersonRepository : IPersonRepository
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMongoCollection<Person> _personCollection;

        public PersonRepository(IPersonRepository personRepository,
            IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _personRepository = personRepository;
            _personCollection = database.GetCollection<Person>(databaseSettings.PersonCollectionName);
        }
        public async Task CreateAsync(Person person)
        {
            await _personCollection.InsertOneAsync(person);

        }
        public async Task<List<Person>> GetAllAsync()
        {
            var persons = await _personCollection.Find(person => true).ToListAsync();

            return persons;
        }

    }

}

