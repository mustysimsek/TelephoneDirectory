using System;
using TelephoneDirectory.ContactReport.Core.Enums;

namespace TelephoneDirectory.ContactReport.Service.Services.Dtos
{
	public class ReportCreateDto
	{
        public DateTime ReportRequestDate { get; set; }
        public string Location { get; set; }
        public int NumberOfRegisteredPersons { get; set; }
        public int NumberOfRegisteredPhones { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}

