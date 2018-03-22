using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace edjCase.SendGrid.WebHooks
{

	public class UnixDateTimeConverter : JsonConverter
	{
		private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			double? unixTimestamp;
			switch (reader.Value)
			{
				case long longValue:
					unixTimestamp = longValue;
					break;
				case double doubleValue:
					unixTimestamp = doubleValue;
					break;
				case string stringValue:
					if (double.TryParse(stringValue, out double value))
					{
						unixTimestamp = value;
					}
					else
					{
						unixTimestamp = null;
					}
					break;
				case null:
					return null;
				default:
					unixTimestamp = null;
					break;
			}
			if (unixTimestamp == null)
			{
				throw new WebHookParseException($"Cannout cast or convert type '{reader.Value.GetType()}' to DateTime.");
			}
			return UnixDateTimeConverter.epoch.AddMilliseconds(unixTimestamp.Value * 1000);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override bool CanWrite => false;
	}

	public class StringListOrItemConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartArray)
			{
				return serializer.Deserialize<List<string>>(reader);
			}
			else
			{
				string item = serializer.Deserialize<string>(reader);
				return new List<string> { item };
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(List<string>);
		}
		public override bool CanWrite => false;
	}

	public class EventTypeConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
		public override bool CanWrite => false;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			string eventType = (string)reader.Value;
			switch (eventType)
			{
				case EventTypeConstants.ProcessedString:
					return EventType.Processed;
				case EventTypeConstants.DeferredString:
					return EventType.Deferred;
				case EventTypeConstants.DeliveredString:
					return EventType.Delivered;
				case EventTypeConstants.OpenString:
					return EventType.Open;
				case EventTypeConstants.ClickString:
					return EventType.Click;
				case EventTypeConstants.BounceString:
					return EventType.Bounce;
				case EventTypeConstants.DroppedString:
					return EventType.Dropped;
				case EventTypeConstants.SpamReportString:
					return EventType.SpamReport;
				case EventTypeConstants.UnsubscribeString:
					return EventType.Unsubscribe;
				case EventTypeConstants.GroupUnsubscribeString:
					return EventType.GroupUnsubscribe;
				case EventTypeConstants.GroupResubscribeString:
					return EventType.GroupResubscribe;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType);
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(EventType);
		}
	}

	public class WebHookEventConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jObject = JObject.Load(reader);
			string eventType = jObject["event"].Value<string>();
			WebHookEvent webHookEvent;
			switch (eventType)
			{
				case EventTypeConstants.ProcessedString:
					webHookEvent = jObject.ToObject<ProcessedEvent>(serializer);
					break;
				case EventTypeConstants.DeferredString:
					webHookEvent = jObject.ToObject<DeferredEvent>(serializer);
					break;
				case EventTypeConstants.DeliveredString:
					webHookEvent = jObject.ToObject<DeliveredEvent>(serializer);
					break;
				case EventTypeConstants.OpenString:
					webHookEvent = jObject.ToObject<OpenEvent>(serializer);
					break;
				case EventTypeConstants.ClickString:
					webHookEvent = jObject.ToObject<ClickEvent>(serializer);
					break;
				case EventTypeConstants.BounceString:
					webHookEvent = jObject.ToObject<BounceEvent>(serializer);
					break;
				case EventTypeConstants.DroppedString:
					webHookEvent = jObject.ToObject<DroppedEvent>(serializer);
					break;
				case EventTypeConstants.SpamReportString:
					webHookEvent = jObject.ToObject<SpamEvent>(serializer);
					break;
				case EventTypeConstants.UnsubscribeString:
					webHookEvent = jObject.ToObject<UnsubscribeEvent>(serializer);
					break;
				case EventTypeConstants.GroupUnsubscribeString:
					webHookEvent = jObject.ToObject<GroupUnsubscribeEvent>(serializer);
					break;
				case EventTypeConstants.GroupResubscribeString:
					webHookEvent = jObject.ToObject<GroupResubscribeEvent>(serializer);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(eventType), eventType);
			}
			foreach (JProperty jProperty in jObject.Properties())
			{
				webHookEvent.RawProperties.Add(jProperty.Name, jProperty.Value);
			}
			return webHookEvent;
		}

		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(WebHookEvent);
		}
	}
}
