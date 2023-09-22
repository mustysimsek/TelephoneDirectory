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
            await GenerateReport(report);
        }

        public async Task<Shared.Dtos.Response<List<Report>>> GetAllAsync()
        {
            var reports = await _reportCollection.Find(report => true).SortByDescending(report => report.ReportRequestDate).ToListAsync();
            if (reports == null)
            {
                return Shared.Dtos.Response<List<Report>>.Fail("There is no any report", 404);
            }

            return Shared.Dtos.Response<List<Report>>.Success(reports, 200);
        }

        public async Task<Shared.Dtos.Response<Report>> GetByIdAsync(string id)
        {
            var report = await _reportCollection.Find<Report>(x => x.UUID == id).FirstOrDefaultAsync();
            if (report == null)
            {
                return Shared.Dtos.Response<Report>.Fail("Report not found", 404);
            }

            return Shared.Dtos.Response<Report>.Success(report, 200);
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
                ReportStatus = ReportStatus.Completed
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
    }
}

