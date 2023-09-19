using System;
using TelephoneDirectory.PersonContact.Core.Common;

namespace TelephoneDirectory.PersonContact.Core.Entities
{
	public class Person : Document
	{
        public string Name { get; set; }
        public string? Surname { get; set; }
        public string? Company { get; set; }
        //public IList<PersonContactInfo>? PersonContactInfo { get; set; }
    }
}

