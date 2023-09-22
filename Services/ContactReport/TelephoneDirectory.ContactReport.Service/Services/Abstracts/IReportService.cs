using System;
using TelephoneDirectory.ContactReport.Core.Entities;
using TelephoneDirectory.ContactReport.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.ContactReport.Service.Services.Abstracts
{
	public interface IReportService
	{
        Task<Response<List<Report>>> GetAllAsync();
        Task<Response<Report>> GetByIdAsync(string id);
        Task<Response<ReportDto>> GenerateReport(ReportDto reportDto);
        //Task<Response<ReportDto>> UpdateGenerateReport(ReportDto reportDto);
    }
}

