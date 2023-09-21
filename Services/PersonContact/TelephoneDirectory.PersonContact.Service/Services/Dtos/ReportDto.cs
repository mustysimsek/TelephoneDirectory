using System;
namespace TelephoneDirectory.PersonContact.Service.Services.Dtos
{
	public class ReportDto
	{
        public string Id { get; set; }
        public DateTime ReportRequestDate { get; set; }
        public string Location { get; set; }
        public int NumberOfRegisteredPersons { get; set; }
        public int NumberOfRegisteredPhones { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }

    public enum ReportStatus
    {
        Preparing = 1,
        Completed = 2
    }
}

