using System;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using TelephoneDirectory.ContactReport.Core.Entities;
using TelephoneDirectory.ContactReport.Core.Enums;
using TelephoneDirectory.ContactReport.Repository.Configurations;
using TelephoneDirectory.ContactReport.Service.Services.Abstracts;
using TelephoneDirectory.ContactReport.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;
using TelephoneDirectory.Shared.Messages;

namespace TelephoneDirectory.ContactReport.Service.Services.Concretes
{
    public class ReportService : IReportService, IConsumer<CreateReportMessageCommand>
    {
        private readonly IMongoCollection<Report> _reportCollection;

        public ReportService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reportCollection = database.GetCollection<Report>(databaseSettings.ReportCollectionName);
        }

        public async Task Consume(ConsumeContext<CreateReportMessageCommand> context)
        {
            var message = context.Message;

            var report = new ReportDto
            {
                ReportRequestDate = message.ReportRequestDate,
                Location = message.Location,
                NumberOfRegisteredPersons = message.NumberOfRegisteredPersons,
                NumberOfRegisteredPhones = message.NumberOfRegisteredPhones,
                ReportStatus = (ReportStatus)message.ReportStatus
            };

            if (report.ReportStatus == ReportStatus.Preparing)
            {
                await GenerateReport(report);
            }
            else
            {
                await UpdateGenerateReport(report);
            }
        }

        public async Task<Shared.Dtos.Response<ReportDto>> CreateAsync(ReportCreateDto reportCreateDto)
        {
            var newReport = reportCreateDto.Adapt<Report>();

            await _reportCollection.InsertOneAsync(newReport);

            return Shared.Dtos.Response<ReportDto>.Success(newReport.Adapt<ReportDto>(), 200);
        }

        public async Task<Shared.Dtos.Response<List<ReportDto>>> GetAllAsync()
        {
            var reports = await _reportCollection.Find(report => true).ToListAsync();

            return Shared.Dtos.Response<List<ReportDto>>.Success(reports.Adapt<List<ReportDto>>(), 200);
        }

        public async Task<Shared.Dtos.Response<Report>> GetByIdAsync(string id)
        {
            var report = await _reportCollection.Find<Report>(x => x.UUID == id).FirstOrDefaultAsync();

            return Shared.Dtos.Response<Report>.Success(report, 200);
        }

        public async Task<Shared.Dtos.Response<ReportDto>> GetByLocationAsync(string location)
        {
            var report = await _reportCollection.Find<Report>(x => x.Location == location).FirstOrDefaultAsync();

            return Shared.Dtos.Response<ReportDto>.Success(report.Adapt<ReportDto>(), 200);
        }

        public async Task<Shared.Dtos.Response<NoContent>> UpdateAsync(Report report)
        {
            var result = await _reportCollection.FindOneAndReplaceAsync(x => x.UUID == report.UUID, report);

            if (result == null)
            {
                return Shared.Dtos.Response<NoContent>.Fail("Report not found", 404);
            }

            return Shared.Dtos.Response<NoContent>.Success(204);
        }

        public async Task<Shared.Dtos.Response<ReportDto>> GenerateReport(ReportDto reportDto)
        {
            var reportEntity = new Report
            {
                CreateDate = DateTime.Now,
                UUID = reportDto.Id,
                ModifiedDate = DateTime.Now,
                ReportRequestDate = reportDto.ReportRequestDate,
                Location = reportDto.Location,
                NumberOfRegisteredPersons = reportDto.NumberOfRegisteredPersons,
                NumberOfRegisteredPhones = reportDto.NumberOfRegisteredPhones,
                ReportStatus = reportDto.ReportStatus
            };

            await _reportCollection.InsertOneAsync(reportEntity);

            //Burada kontrol eklenebilir

            var responseModel = new ReportDto
            {
                ReportRequestDate = reportEntity.ReportRequestDate,
                Location = reportEntity.Location,
                NumberOfRegisteredPersons = reportEntity.NumberOfRegisteredPersons,
                NumberOfRegisteredPhones = reportEntity.NumberOfRegisteredPhones,
                ReportStatus = reportEntity.ReportStatus
            };

            return Shared.Dtos.Response<ReportDto>.Success(responseModel, 200);
        }

        public async Task<Shared.Dtos.Response<ReportDto>> UpdateGenerateReport(ReportDto reportDto)
        {
            var reportEntity = new Report
            {
                CreateDate = DateTime.Now,
                UUID = reportDto.Id,
                ModifiedDate = DateTime.Now,
                ReportRequestDate = reportDto.ReportRequestDate,
                Location = reportDto.Location,
                NumberOfRegisteredPersons = reportDto.NumberOfRegisteredPersons,
                NumberOfRegisteredPhones = reportDto.NumberOfRegisteredPhones,
                ReportStatus = reportDto.ReportStatus
            };

            var reportState = _reportCollection.
                Find<Report>(x => x.Location == reportDto.Location &&
                x.ReportRequestDate == reportDto.ReportRequestDate &&
                x.ReportStatus == ReportStatus.Preparing).ToList();

            reportEntity.UUID = reportState[0].UUID;

            var filter = Builders<Report>.Filter.Eq(doc => doc.UUID, reportDto.Id);
            await _reportCollection.FindOneAndReplaceAsync(filter, reportEntity);

            return Shared.Dtos.Response<ReportDto>.Success(200);
        }
    }
}

