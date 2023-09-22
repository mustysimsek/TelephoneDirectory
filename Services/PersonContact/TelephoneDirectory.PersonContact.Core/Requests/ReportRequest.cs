using System;
using TelephoneDirectory.PersonContact.Core.Enums;

namespace TelephoneDirectory.PersonContact.Core.Requests
{
	public class ReportRequest
	{
        public string Id { get; set; }
        public DateTime ReportRequestDate { get; set; }
        public string Location { get; set; }
        public int NumberOfRegisteredPersons { get; set; }
        public int NumberOfRegisteredPhones { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Preparing;
    }
}

