using System;
namespace TelephoneDirectory.ContactReport.Repository.Configurations
{
	public interface IDatabaseSettings
	{
        public string ReportCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

