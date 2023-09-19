﻿using System;
namespace TelephoneDirectory.PersonContact.Repository.Configurations
{
	public interface IDatabaseSettings
	{
        public string PersonCollectionName { get; set; }
        public string PersonContactInfoCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}

