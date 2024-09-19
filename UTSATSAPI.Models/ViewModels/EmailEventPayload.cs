using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class EmailEventPayload
    {
        public string EventType { get; set; }
        public Mail Mail { get; set; }
        public Send Send { get; set; }
        public Delivery Delivery { get; set; }
        public Open Open { get; set; }
        public Click Click { get; set; }
        public BounceDetails Bounce { get; set; }
    }

    public class Mail
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
        public string SourceArn { get; set; }
        public string SendingAccountId { get; set; }
        public string MessageId { get; set; }
        public List<string> Destination { get; set; }
        public bool HeadersTruncated { get; set; }
        public List<Header> Headers { get; set; }
        public CommonHeaders CommonHeaders { get; set; }
        public Tags Tags { get; set; }
    }

    public class Header
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CommonHeaders
    {
        public List<string> From { get; set; }
        public List<string> To { get; set; }
        public string MessageId { get; set; }
        public string Subject { get; set; }
    }

    public class Tags
    {
        public List<string> SourceTlsVersion { get; set; }
        public List<string> Operation { get; set; }
        public List<string> ConfigurationSet { get; set; }
        public List<string> RecipientIsp { get; set; }
        public List<string> SourceIp { get; set; }
        public List<string> FromDomain { get; set; }
        public List<string> SenderIdentity { get; set; }
        public List<string> CallerIdentity { get; set; }
        public List<string> OutgoingIp { get; set; }
    }

    public class Send { }

    public class Delivery
    {
        public DateTime Timestamp { get; set; }
        public long ProcessingTimeMillis { get; set; }
        public List<string> Recipients { get; set; }
        public string SmtpResponse { get; set; }
        public string ReportingMTA { get; set; }
    }

    public class Open
    {
        public DateTime Timestamp { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
    }

    public class Click
    {
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Link { get; set; }
        public object LinkTags { get; set; }
    }

    public class BounceDetails
    {
        public string FeedbackId { get; set; }
        public string BounceType { get; set; }
        public string BounceSubType { get; set; }
        public List<BouncedRecipient> BouncedRecipients { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReportingMTA { get; set; }

        public class BouncedRecipient
        {
            public string EmailAddress { get; set; }
            public string Action { get; set; }
            public string Status { get; set; }
            public string DiagnosticCode { get; set; }
        }
    }
}
