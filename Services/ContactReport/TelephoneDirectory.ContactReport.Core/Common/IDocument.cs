using System;
using System.ComponentModel.DataAnnotations;

namespace TelephoneDirectory.ContactReport.Core.Common
{
	public interface IDocument
	{
        string UUID { get; set; }
        DateTime CreateDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}

