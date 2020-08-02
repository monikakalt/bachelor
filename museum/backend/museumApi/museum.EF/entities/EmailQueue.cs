using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace museum.EF.entities
{
        public class EmailQueue
        {
            public long Id { get; set; }
            public string Status { get; set; }
            public string Error { get; set; }
            public int RetryCount { get; set; }
            public DateTime? LastStateDate { get; set; }
            public string EmailRecipientAdress { get; set; }
            public string EmailSubject { get; set; }
            public string EmailText { get; set; }
            public string EmailTemplateCode { get; set; }
            public string EmailAttachmentName { get; set; }
            public byte[] EmailAttachment { get; set; }
        }
}
