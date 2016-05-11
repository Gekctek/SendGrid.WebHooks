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
		public string EmailAddress { get; private set; }
		[JsonProperty("timestamp")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime TimeStamp { get; private set; }
		[JsonProperty("category")]
		[JsonConverter(typeof(StringListOrItemConverter))]
		public List<string> Category { get; private set; }
		[JsonProperty("event")]
		[JsonConverter(typeof(EventTypeConverter))]
		public EventType Event { get; private set; }
		[JsonProperty("sg_event_id")]
		public string InternalEventId { get; private set; }
		[JsonProperty("sg_message_id")]
		public string InternalMessageId { get; private set; }
		[JsonIgnore]
		public Dictionary<string, JToken> RawProperties { get; private set; } = new Dictionary<string, JToken>();
	}

	public class BounceEvent : WebHookEvent
	{
		[JsonProperty("status")]
		public string Status { get; private set; }
		[JsonProperty("smtp-id")]
		public string SmtpId { get; private set; }
		[JsonProperty("newletter")]
		public EventNewsletter Newsletter { get; private set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }
		[JsonProperty("reason")]
		public string Reason { get; private set; }
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }
		[JsonProperty("tls")]
		public bool? Tls { get; private set; }
		[JsonProperty("cert_err")]
		public bool? CertificateError { get; private set; }
	}

	public class ClickEvent : WebHookEvent
	{
		[JsonProperty("url")]
		public string Url { get; private set; }
		[JsonProperty("url_offset")]
		public string UrlOffset { get; private set; }
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; private set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; private set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }
	}

	public class DeferredEvent : WebHookEvent
	{
		[JsonProperty("response")]
		public string Response { get; private set; }

		[JsonProperty("attempt")]
		public int Attempt { get; private set; }

		[JsonProperty("smtp-id")]
		public string SmtpId { get; private set; }

		[JsonProperty("ip")]
		public string IpAddress { get; private set; }

		[JsonProperty("tls")]
		public bool? Tls { get; private set; }

		[JsonProperty("cert_err")]
		public bool? CertificateError { get; private set; }

		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }

		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; private set; }

	}
	public class DeliveredEvent : WebHookEvent
	{
		[JsonProperty("response")]
		public string Response { get; private set; }
		[JsonProperty("smtp-id")]
		public string SmtpId { get; private set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }

		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; private set; }
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }

		[JsonProperty("tls")]
		public bool? Tls { get; private set; }

		[JsonProperty("cert_err")]
		public bool? CertificateError { get; private set; }
	}

	public class DroppedEvent : WebHookEvent
	{
		[JsonProperty("smtp-id")]
		public string SmtpId { get; private set; }
		[JsonProperty("reason")]
		public string Reason { get; private set; }
	}
	public class OpenEvent : WebHookEvent
	{
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; private set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; private set; }
	}

	public class ProcessedEvent : WebHookEvent
	{
		[JsonProperty("smtp-id")]
		public string SmtpId { get; private set; }
		[JsonProperty("asm_group_id")]
		public int? AsmGroupId { get; private set; }
		[JsonProperty("newsletter")]
		public EventNewsletter Newsletter { get; private set; }
		[JsonProperty("send_at")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? SendAt { get; private set; }
	}




	public class SpamEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; private set; }
	}

	public class UnsubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; private set; }
	}

	public class GroupUnsubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; private set; }
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; private set; }
	}

	public class GroupResubscribeEvent : WebHookEvent
	{
		[JsonProperty("asm_group_id")]
		public int AsmGroupId { get; private set; }
		[JsonProperty("ip")]
		public string IpAddress { get; private set; }
		[JsonProperty("useragent")]
		public string UserAgent { get; private set; }
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
		public string UserListId { get; private set; }
		[JsonProperty("newsletter_id", Required = Required.Always)]
		public string Id { get; private set; }
		[JsonProperty("newsletter_send_id", Required = Required.Always)]
		public string SendId { get; private set; }
	}
}
