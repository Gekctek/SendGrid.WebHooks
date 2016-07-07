using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using edjCase.SendGrid.WebHooks.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace edjCase.SendGrid.WebHooks
{
	public class WebHookParser : IWebHookParser
	{
		public async Task<List<WebHookEvent>> ParseAsync(HttpContext httpContext)
		{
			string requestBody;
			using (var streamReader = new StreamReader(httpContext.Request.Body))
			{
				requestBody = await streamReader.ReadToEndAsync();
			}
			var webHookEvent = JsonConvert.DeserializeObject<List<WebHookEvent>>(requestBody, new WebHookEventConverter());

			return webHookEvent;
		}
	}
}
