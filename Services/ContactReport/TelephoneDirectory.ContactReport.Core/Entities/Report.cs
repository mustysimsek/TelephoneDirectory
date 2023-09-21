using System;
using TelephoneDirectory.ContactReport.Core.Common;
using TelephoneDirectory.ContactReport.Core.Enums;

namespace TelephoneDirectory.ContactReport.Core.Entities
{
	public class Report : Document
	{
        public DateTime ReportRequestDate { get; set; }
        public string Location { get; set; }
        public int NumberOfRegisteredPersons { get; set; }
        public int NumberOfRegisteredPhones { get; set; }
        public ReportStatus ReportStatus { get; set; }
    }
}

