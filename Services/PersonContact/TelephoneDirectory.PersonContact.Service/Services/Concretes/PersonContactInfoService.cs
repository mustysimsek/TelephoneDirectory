using System;
using System.Linq;
using Mapster;
using MongoDB.Driver;
using TelephoneDirectory.PersonContact.Core.Entities;
using TelephoneDirectory.PersonContact.Core.Enums;
using TelephoneDirectory.PersonContact.Repository.Configurations;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;
using TelephoneDirectory.Shared.Interfaces;
using TelephoneDirectory.Shared.Messages;

namespace TelephoneDirectory.PersonContact.Service.Services.Concretes
{
    public class PersonContactInfoService : IPersonContactInfoService
    {
        private readonly IMongoCollection<PersonContactInfo> _personContactInfoCollection;
        private readonly IMongoCollection<Person> _personCollection;
        private readonly IRabbitMqService _rabbitMqService;

        public PersonContactInfoService(IDatabaseSettings databaseSettings, IRabbitMqService rabbitMqService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _personContactInfoCollection = database.GetCollection<PersonContactInfo>
                (databaseSettings.PersonContactInfoCollectionName);
            _personCollection = database.GetCollection<Person>(databaseSettings.PersonCollectionName);
            _rabbitMqService = rabbitMqService;
        }

        public async Task<Response<PersonContactInfo>> CreateAsync(PersonContactInfoDto personContactInfoDto)
        {
            var newPersonContactInfo = personContactInfoDto.Adapt<PersonContactInfo>();
            newPersonContactInfo.CreateDate = DateTime.Now;

            await _personContactInfoCollection.InsertOneAsync(newPersonContactInfo);

            return Response<PersonContactInfo>.Success(personContactInfoDto.Adapt<PersonContactInfo>(), 200);
        }

        public async Task<Response<NoContent>> DeleteAsync(string personContactInfoUuid)
        {
            var result = await _personContactInfoCollection.DeleteOneAsync(x => x.UUID == personContactInfoUuid);

            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);
            }
            else
            {
                return Response<NoContent>.Fail("There is no Person Contact Info with this ID", 404);
            }
        }

        public async Task<Response<ReportDto>> GetReportByLocation(ReportDto reportDto)
        {
            var response = new Response<ReportDto>();
            var reportRequest = reportDto;

            var personsAtLocation = await _personContactInfoCollection.
            Find<PersonContactInfo>(x => x.Content == reportDto.Location.ToLower() && x.PersonContactInfoType == Core.Enums.PersonContactInfoType.Location).ToListAsync();
            var uniquePersonIds = personsAtLocation.Select(person => person.PersonId).Distinct().ToList();
            var personNumbersAtLocation = uniquePersonIds.Count();

            var phoneNumbersForUniquePersons = await _personContactInfoCollection
            .Find<PersonContactInfo>(x => uniquePersonIds.Contains(x.PersonId)
            && x.PersonContactInfoType == Core.Enums.PersonContactInfoType.PhoneNumber).ToListAsync();
            var uniquePhoneNumbersforPersonIds = phoneNumbersForUniquePersons.Select(person => person.PersonId).Distinct().ToList();
            var registeredPhoneNumbersAtLocation = uniquePhoneNumbersforPersonIds.Count();

            response.Data = reportRequest.Adapt<ReportDto>();

            if (response == null)
            {
                return Response<ReportDto>.Fail("There are no registered users at this location", 404);
            }

            //reportRequest.ReportStatus = Dtos.ReportStatus.Preparing;
            reportRequest.NumberOfRegisteredPersons = personNumbersAtLocation;
            reportRequest.NumberOfRegisteredPhones = registeredPhoneNumbersAtLocation;

            response.Data = reportRequest;

            return Response<ReportDto>.Success(reportRequest, 200);
        }

    }

}

