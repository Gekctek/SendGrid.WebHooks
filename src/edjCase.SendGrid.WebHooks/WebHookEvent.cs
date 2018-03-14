using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace edjCase.SendGrid.WebHooks
{
	public abstract class WebHookEvent
	{
		[JsonProperty("email")]
		public string EmailAddress { get; set; }
		[JsonProperty("timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime TimeStamp { get; set; }
		[JsonProperty("category")]
		[JsonConverter(typeof(StringListOrItemConverter))]
		public List<string> Category { get; set; }
		[JsonProperty("event")]
		[JsonConverter(typeof(EventTypeConverter))]
		public EventType Event { get; set; }
		[JsonProperty("sg_event_id")]
		public string InternalEventId { get; set; }
		[JsonProperty("sg_message_id")]
		public string InternalMessageId { get; set; }
		[JsonIgnore]
		public Dictionary<string, JToken> RawProperties { get; set; } = new Dictionary<string, JToken>();
	}

	public class BounceEvent : WebHookEvent
	{

		public BounceEvent()
		{
			
		}
		[JsonProperty("status")]
		public string Status { get; set; }
		[JsonProperty("smtp-id")]
		public string SmtpId { get; set; }
		[JsonProperty("newletter")]
		public EventNewsletter Newsletter { get; set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }
		[JsonProperty("reason")]
		public string Reason { get; set; }
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		[JsonProperty("tls")]
		public bool? Tls { get; set; }
		[JsonProperty("cert_err")]
		public bool? CertificateError { get; set; }
	}

	public class ClickEvent : WebHookEvent
	{
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("url_offset")]
		public UrlOffset UrlOffset { get; set; }
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }
	}

	public class DeferredEvent : WebHookEvent
	{
		[JsonProperty("response")]
		public string Response { get; set; }

		[JsonProperty("attempt")]
		public int Attempt { get; set; }

		[JsonProperty("smtp-id")]
		public string SmtpId { get; set; }

		[JsonProperty("ip")]
		public string IpAddress { get; set; }

		[JsonProperty("tls")]
		public bool? Tls { get; set; }

		[JsonProperty("cert_err")]
		public bool? CertificateError { get; set; }

		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }

		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; set; }

	}
	public class DeliveredEvent : WebHookEvent
	{
		[JsonProperty("response")]
		public string Response { get; set; }
		[JsonProperty("smtp-id")]
		public string SmtpId { get; set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }

		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; set; }
		[JsonProperty("ip")]
		public string IpAddress { get; set; }

		[JsonProperty("tls")]
		public bool? Tls { get; set; }

		[JsonProperty("cert_err")]
		public bool? CertificateError { get; set; }
	}

	public class DroppedEvent : WebHookEvent
	{
		[JsonProperty("smtp-id")]
		public string SmtpId { get; set; }
		[JsonProperty("reason")]
		public string Reason { get; set; }
	}
	public class OpenEvent : WebHookEvent
	{
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; set; }
	}

	public class ProcessedEvent : WebHookEvent
	{
		[JsonProperty("smtp-id")]
		public string SmtpId { get; set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; set; }
		[JsonProperty("send_at")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? SendAt { get; set; }
	}




	public class SpamEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; set; }
	}

	public class UnsubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; set; }
	}

	public class GroupUnsubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; set; }
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; set; }
	}

	public class GroupResubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; set; }
		[JsonProperty("ip")]
		public string IpAddress { get; set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; set; }
	}

	public enum EventType
	{
		Processed = 0,
		Deferred = 1,
		Delivered = 2,
		Open = 3,
		Click = 4,
		Bounce = 5,
		Dropped = 6,
		SpamReport = 7,
		Unsubscribe = 8,
		GroupUnsubscribe = 9,
		GroupResubscribe = 10
	}

	public static class EventTypeConstants
	{
		public const string ProcessedString = "processed";
		public const string DeferredString = "deferred";
		public const string DeliveredString = "delivered";
		public const string OpenString = "open";
		public const string ClickString = "click";
		public const string BounceString = "bounce";
		public const string DroppedString = "dropped";
		public const string SpamReportString = "spamreport";
		public const string UnsubscribeString = "unsubscribe";
		public const string GroupUnsubscribeString = "group_unsubscribe";
		public const string GroupResubscribeString = "group_resubscribe";
	}

	public class EventNewsletter
	{
		[JsonProperty("newsletter_user_list_id", Required = Required.Always)]
		public string UserListId { get; set; }
		[JsonProperty("newsletter_id", Required = Required.Always)]
		public string Id { get; set; }
		[JsonProperty("newsletter_send_id", Required = Required.Always)]
		public string SendId { get; set; }
	}
	
	public class UrlOffset
    	{
        	[JsonProperty("index")]
        	public int Index { get; set; }
        	[JsonProperty("type")]
        	public string Type { get; set; }
        }
}
