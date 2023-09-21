using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.ContactReport.Service.Services.Abstracts;
using TelephoneDirectory.ContactReport.Service.Services.Dtos;
using TelephoneDirectory.Shared.ControllerBases;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TelephoneDirectory.ContactReport.API.Controllers
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportController : CustomBaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _reportService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        //reports/1
        [HttpGet, Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _reportService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ReportCreateDto reportCreateDto)
        {
            var response = await _reportService.CreateAsync(reportCreateDto);

            return CreateActionResultInstance(response);
        }
    }
}

