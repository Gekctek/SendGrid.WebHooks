using System;
using System.Threading.Tasks;

namespace edjCase.SendGrid.WebHooks.Abstractions
{
	public interface IWebHookProcessor
	{
		Task OnProcessedAsync(ProcessedEvent processedEvent);
		Task OnDelivieredAsync(DeliveredEvent deliveredEvent);
		Task OnDroppedAsync(DroppedEvent droppedEvent);
		Task OnBouncedAsync(BounceEvent bounceEvent);
		Task OnClickAsync(ClickEvent clickEvent);
		Task OnOpenAsync(OpenEvent openEvent);
		Task OnDeferredAsync(DeferredEvent deferredEvent);
		Task OnSpamReportAsync(SpamEvent spamEvent);
		Task OnUnsubscribeAsync(UnsubscribeEvent unsubscribeEvent);
		Task OnGroupUnsubscribeEvent(GroupUnsubscribeEvent groupUnsubscribeEvent);
		Task OnGroupResubscribeEvent(GroupResubscribeEvent groupResubscribeEvent);
		Task<bool> HandleException(WebHookEvent webHookEvent, Exception ex);
	}

	public static class WebHookProcessorExtensions
	{
		public static Task Run(this IWebHookProcessor webHookProcessor, WebHookEvent webHookEvent)
		{
			if (webHookEvent is DeliveredEvent)
			{
				return webHookProcessor.OnDelivieredAsync((DeliveredEvent) webHookEvent);
			}
			if (webHookEvent is ProcessedEvent)
			{
				return webHookProcessor.OnProcessedAsync((ProcessedEvent)webHookEvent);
			}
			if (webHookEvent is OpenEvent)
			{
				return webHookProcessor.OnOpenAsync((OpenEvent)webHookEvent);
			}
			if (webHookEvent is ClickEvent)
			{
				return webHookProcessor.OnClickAsync((ClickEvent)webHookEvent);
			}
			if (webHookEvent is SpamEvent)
			{
				return webHookProcessor.OnSpamReportAsync((SpamEvent)webHookEvent);
			}
			if (webHookEvent is DeferredEvent)
			{
				return webHookProcessor.OnDeferredAsync((DeferredEvent)webHookEvent);
			}
			if (webHookEvent is DroppedEvent)
			{
				return webHookProcessor.OnDroppedAsync((DroppedEvent)webHookEvent);
			}
			if (webHookEvent is BounceEvent)
			{
				return webHookProcessor.OnBouncedAsync((BounceEvent)webHookEvent);
			}
			if (webHookEvent is UnsubscribeEvent)
			{
				return webHookProcessor.OnUnsubscribeAsync((UnsubscribeEvent)webHookEvent);
			}
			if (webHookEvent is GroupUnsubscribeEvent)
			{
				return webHookProcessor.OnGroupUnsubscribeEvent((GroupUnsubscribeEvent)webHookEvent);
			}
			if (webHookEvent is GroupResubscribeEvent)
			{
				return webHookProcessor.OnGroupResubscribeEvent((GroupResubscribeEvent)webHookEvent);
			}
			throw new ArgumentOutOfRangeException(nameof(webHookEvent));
		}
	}
}