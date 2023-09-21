using System;
using TelephoneDirectory.Shared.Dtos;
using TelephoneDirectory.Shared.Enums;

namespace TelephoneDirectory.Shared.Messages
{
	public class CreateReportMessageCommand
	{
        //public string Location { get; set; }
        public string ReportId { get; set; }
        public DateTime ReportRequestDate { get; set; }
        public string Location { get; set; }
        public int NumberOfRegisteredPersons { get; set; }
        public int NumberOfRegisteredPhones { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }

}

