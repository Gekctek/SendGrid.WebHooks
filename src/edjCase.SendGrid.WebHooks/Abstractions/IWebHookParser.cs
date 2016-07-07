using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace edjCase.SendGrid.WebHooks.Abstractions
{
	public interface IWebHookParser
	{
		Task<List<WebHookEvent>> ParseAsync(HttpContext httpContext);
	}
}