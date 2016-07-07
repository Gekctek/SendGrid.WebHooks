using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edjCase.SendGrid.WebHooks;
using edjCase.SendGrid.WebHooks.Abstractions;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
	public static class WebHookExtensions
	{
		public static IServiceCollection AddSendGridWebHook(this IServiceCollection serviceCollection)
		{
			return serviceCollection
				.AddScoped<IWebHookParser, WebHookParser>();
		}

		public static IServiceCollection AddSendGridWebHookWithHandler<THandler>(this IServiceCollection serviceCollection)
			where THandler : class, IWebHookHandler
		{
			return serviceCollection
				.AddScoped<IWebHookParser, WebHookParser>()
				.AddScoped<IWebHookHandler, THandler>();
		}

		public static IServiceCollection AddSendGridWebHookWithHandler(this IServiceCollection serviceCollection, Func<IServiceProvider, IWebHookHandler> webHookHandlerFactory)
		{
			return serviceCollection
				.AddScoped<IWebHookParser, WebHookParser>()
				.AddScoped(webHookHandlerFactory);
		}

		public static IApplicationBuilder UseSendGridWebHookWithHandler(this IApplicationBuilder app)
		{
			WebHookExtensions.ValidateWebHookParser(app);
			WebHookExtensions.ValidateWebHookHandler(app);
			return app.Use(async (httpContext, next) =>
			{
				var webHookParser = app.ApplicationServices.GetRequiredService<IWebHookParser>();
				var webHookHandler = app.ApplicationServices.GetRequiredService<IWebHookHandler>();

				var unhandledErrors = new List<Exception>();
				List<WebHookEvent> webHookEvents = await webHookParser.ParseAsync(httpContext);
				foreach (WebHookEvent webHookEvent in webHookEvents)
				{
					try
					{
						await webHookHandler.Run(webHookEvent);
					}
					catch (Exception ex1)
					{
						try
						{
							bool handled = await webHookHandler.HandleException(webHookEvent, ex1);
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
				WebHookExtensions.HandleUnhandledErrors(unhandledErrors);
			});
		}

		public static IApplicationBuilder UseSendGridWebHook(this IApplicationBuilder app, Func<WebHookEvent, Task> processEvent, Func<WebHookEvent, Exception, Task<bool>> handleException)
		{
			WebHookExtensions.ValidateWebHookParser(app);
			return app.Use(async (httpContext, next) =>
			{
				var webHookParser = app.ApplicationServices.GetRequiredService<IWebHookParser>();
				List<WebHookEvent> webHookEvents = await webHookParser.ParseAsync(httpContext);

				var unhandledErrors = new List<Exception>();
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
				WebHookExtensions.HandleUnhandledErrors(unhandledErrors);
			});
		}

		private static void HandleUnhandledErrors(List<Exception> unhandledErrors)
		{
			if (unhandledErrors.Any())
			{
				throw new WebHookProcessingException("There were unhanled exceptions while processing the webhook.", unhandledErrors);
			}
		}

		private static void ValidateWebHookParser(IApplicationBuilder app)
		{
			var webHookParser = app.ApplicationServices.GetService<IWebHookParser>();
			if (webHookParser != null)
			{
				return;
			}
			const string errorMessage = "Unable to find the required service IWebHookParser. Please add all the required " +
										"services by calling 'IServiceCollection.AddSendGridWebHook()' inside " +
										"the call to 'IApplicationBuilder.ConfigureServices(...)' or " +
										"'IApplicationBuilder.UseSendGridWebHook(...)' in the application startup code.";
			throw new InvalidOperationException(errorMessage);
		}
		private static void ValidateWebHookHandler(IApplicationBuilder app)
		{
			var webHookHandler = app.ApplicationServices.GetService<IWebHookHandler>();
			if (webHookHandler != null)
			{
				return;
			}
			const string errorMessage = "Unable to find the required service IWebHookHandler. Please add all the required " +
										"services by calling 'IServiceCollection.AddSendGridWebHookWithHandler()' inside " +
										"the call to 'IApplicationBuilder.ConfigureServices(...)' or " +
										"'IApplicationBuilder.UseSendGridWebHookWithHandler(...)' in the application startup code.";
			throw new InvalidOperationException(errorMessage);
		}
	}
}
