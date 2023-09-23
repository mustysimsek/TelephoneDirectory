using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.PersonContact.Core.Requests;
using TelephoneDirectory.PersonContact.Service.Services.Abstracts;
using TelephoneDirectory.PersonContact.Service.Services.Concretes;
using TelephoneDirectory.PersonContact.Service.Services.Dtos;
using TelephoneDirectory.Shared.ControllerBases;
using TelephoneDirectory.Shared.Interfaces;
using TelephoneDirectory.Shared.Messages;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelephoneDirectory.PersonContact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonContactInfoController : CustomBaseController
    {
        private readonly IPersonContactInfoService _personContactInfoService;
        private readonly IRabbitMqService _rabbitMqService;

        public PersonContactInfoController(IPersonContactInfoService personContactInfoService, IRabbitMqService rabbitMqService)
        {
            _personContactInfoService = personContactInfoService;
            _rabbitMqService = rabbitMqService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonContactInfoDto request)
        {
            var response = await _personContactInfoService.CreateAsync(request);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _personContactInfoService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetReportByLocation(string location)
        {
            var createReportMessageCommand = new CreateReportMessageCommand();

            var reportRequest = new ReportDto
            {
                Location = location,
                ReportRequestDate = DateTime.Now,
                ReportStatus = ReportStatus.Preparing
            };

            var report = await _personContactInfoService.GetReportByLocation(reportRequest);

            createReportMessageCommand.Location = reportRequest.Location;
            createReportMessageCommand.ReportRequestDate = reportRequest.ReportRequestDate;
            createReportMessageCommand.NumberOfRegisteredPersons = report.Data.NumberOfRegisteredPersons;
            createReportMessageCommand.NumberOfRegisteredPhones = report.Data.NumberOfRegisteredPhones;
            createReportMessageCommand.ReportStatus = (Shared.Enums.ReportStatus)report.Data.ReportStatus;

            await _rabbitMqService.SendMessage(createReportMessageCommand);

            return CreateActionResultInstance(report);
        }

        #region OLD

        //[HttpPost]
        //public async Task<IActionResult> ReceiveReport(PersonContactInfoDto request)
        //{
        //    //var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-report-service"));

        //    var createReportMessageCommand = new CreateReportMessageCommand();

        //    createReportMessageCommand.Location = request.Report.Location;
        //    createReportMessageCommand.ReportRequestDate = request.Report.ReportRequestDate;
        //    createReportMessageCommand.ReportStatus = (Shared.Enums.ReportStatus)Core.Enums.ReportStatus.Prepare;

        //    await _rabbitMqService.SendMessage(createReportMessageCommand);

        //    var reportRequest = new ReportDto
        //    {
        //        Location = createReportMessageCommand.Location,
        //        ReportRequestDate = createReportMessageCommand.ReportRequestDate
        //    };

        //    var report = await _personContactInfoService.GetReportByLocation(reportRequest);

        //    createReportMessageCommand.Location = reportRequest.Location;
        //    createReportMessageCommand.ReportRequestDate = reportRequest.ReportRequestDate;
        //    createReportMessageCommand.NumberOfRegisteredPersons = report.Data.NumberOfRegisteredPersons;
        //    createReportMessageCommand.NumberOfRegisteredPhones = report.Data.NumberOfRegisteredPhones;
        //    createReportMessageCommand.ReportStatus = (Shared.Enums.ReportStatus)report.Data.ReportStatus;

        //    await _rabbitMqService.SendMessage(createReportMessageCommand);

        //    return CreateActionResultInstance(report);
        #endregion
    }
}


