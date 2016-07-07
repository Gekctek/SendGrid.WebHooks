using System;
using System.Threading.Tasks;

namespace edjCase.SendGrid.WebHooks.Abstractions
{
	public interface IWebHookHandler
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
		Task OnGroupUnsubscribeAsync(GroupUnsubscribeEvent groupUnsubscribeEvent);
		Task OnGroupResubscribeAsync(GroupResubscribeEvent groupResubscribeEvent);
		Task<bool> HandleExceptionAsync(WebHookEvent webHookEvent, Exception ex);
	}

	public static class WebHookHandlerExtensions
	{
		public static Task Run(this IWebHookHandler webHookHandler, WebHookEvent webHookEvent)
		{
			if (webHookEvent is DeliveredEvent)
			{
				return webHookHandler.OnDelivieredAsync((DeliveredEvent) webHookEvent);
			}
			if (webHookEvent is ProcessedEvent)
			{
				return webHookHandler.OnProcessedAsync((ProcessedEvent)webHookEvent);
			}
			if (webHookEvent is OpenEvent)
			{
				return webHookHandler.OnOpenAsync((OpenEvent)webHookEvent);
			}
			if (webHookEvent is ClickEvent)
			{
				return webHookHandler.OnClickAsync((ClickEvent)webHookEvent);
			}
			if (webHookEvent is SpamEvent)
			{
				return webHookHandler.OnSpamReportAsync((SpamEvent)webHookEvent);
			}
			if (webHookEvent is DeferredEvent)
			{
				return webHookHandler.OnDeferredAsync((DeferredEvent)webHookEvent);
			}
			if (webHookEvent is DroppedEvent)
			{
				return webHookHandler.OnDroppedAsync((DroppedEvent)webHookEvent);
			}
			if (webHookEvent is BounceEvent)
			{
				return webHookHandler.OnBouncedAsync((BounceEvent)webHookEvent);
			}
			if (webHookEvent is UnsubscribeEvent)
			{
				return webHookHandler.OnUnsubscribeAsync((UnsubscribeEvent)webHookEvent);
			}
			if (webHookEvent is GroupUnsubscribeEvent)
			{
				return webHookHandler.OnGroupUnsubscribeAsync((GroupUnsubscribeEvent)webHookEvent);
			}
			if (webHookEvent is GroupResubscribeEvent)
			{
				return webHookHandler.OnGroupResubscribeAsync((GroupResubscribeEvent)webHookEvent);
			}
			throw new ArgumentOutOfRangeException(nameof(webHookEvent));
		}
	}
}