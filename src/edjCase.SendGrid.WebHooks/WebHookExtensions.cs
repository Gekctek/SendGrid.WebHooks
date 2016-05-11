using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edjCase.SendGrid.WebHooks;
using edjCase.SendGrid.WebHooks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNet.Builder
{
	public static class WebHookExtensions
	{
		public static IServiceCollection AddSendGridWebHook(this IServiceCollection serviceCollection)
		{
			return serviceCollection
				.AddScoped<IWebHookParser, WebHookParser>();
		}

		public static IApplicationBuilder UseSendGridWebHook(this IApplicationBuilder app, IWebHookProcessor webHookProcessor)
		{
			return app.UseSendGridWebHook(webHookProcessor.Run, webHookProcessor.HandleException);
		}

		public static IApplicationBuilder UseSendGridWebHook(this IApplicationBuilder app, Func<WebHookEvent, Task> processEvent, Func<WebHookEvent, Exception, Task<bool>> handleException)
		{
			var webHookParser = app.ApplicationServices.GetService<IWebHookParser>();
			if (webHookParser == null)
			{
				const string errorMessage = "Unable to find the required services. Please add all the required " +
											"services by calling 'IServiceCollection.AddSendGridWebHook()' inside " +
											"the call to 'IApplicationBuilder.ConfigureServices(...)' or " +
											"'IApplicationBuilder.UseSendGridWebHook(...)' in the application startup code.";
				throw new InvalidOperationException(errorMessage);
			}
			return app.Use(async (httpContext, next) =>
			{
				var unhandledErrors = new List<Exception>();
				List<WebHookEvent> webHookEvents = await webHookParser.ParseAsync(httpContext);
				foreach (WebHookEvent webHookEvent in webHookEvents)
				{
					try
					{
						await processEvent(webHookEvent);
					}
					catch (Exception ex1)
					{
						try
						{
							bool handled = await handleException(webHookEvent, ex1);
							if (!handled)
							{
								unhandledErrors.Add(ex1);
							}
						}
						catch (Exception ex2)
						{
							unhandledErrors.Add(ex2);
						}
					}
				}
				if (unhandledErrors.Any())
				{
					throw new WebHookProcessingException("There were unhanled exceptions while processing the webhook.", unhandledErrors);
				}
			});
		}

	}
}
