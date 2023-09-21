using System;
using TelephoneDirectory.ContactReport.Core.Entities;
using TelephoneDirectory.ContactReport.Service.Services.Dtos;
using TelephoneDirectory.Shared.Dtos;

namespace TelephoneDirectory.ContactReport.Service.Services.Abstracts
{
	public interface IReportService
	{
        Task<Response<ReportDto>> CreateAsync(ReportCreateDto reportCreateDto);
        Task<Response<List<ReportDto>>> GetAllAsync();
        Task<Response<Report>> GetByIdAsync(string id);
        Task<Response<ReportDto>> GetByLocationAsync(string location);
        Task<Response<NoContent>> UpdateAsync(Report report);
        Task<Response<ReportDto>> GenerateReport(ReportDto reportDto);
        Task<Response<ReportDto>> UpdateGenerateReport(ReportDto reportDto);
    }
}

