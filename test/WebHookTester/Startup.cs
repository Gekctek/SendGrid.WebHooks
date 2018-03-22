using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using edjCase.SendGrid.WebHooks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebHookTester
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSendGridWebHook();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSendGridWebHook(this.Process, this.HandleException);
		}

		private Task<bool> HandleException(WebHookEvent arg1, Exception arg2)
		{
			return Task.FromResult(true);
		}

		private Task Process(WebHookEvent arg)
		{
			return Task.CompletedTask;
		}
	}
}
