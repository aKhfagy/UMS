using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UMS.Core.Dtos;
using UMS.Core.Interfaces;
using UMS.Core.Interfaces.Repositories;

namespace UMS.Application.Middlewares
{
	public class LoggerMiddleware
	{
		private readonly RequestDelegate _next;
		public LoggerMiddleware(
			RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			var log = new HttplogDto();
			log.StatusCode = httpContext.Response.StatusCode;
			log.Date = DateTime.Now;
			log.Type = httpContext.Request.Method;
			log.Request = $@"{httpContext.Request.Host}{httpContext.Request.Path}";
			var request = httpContext.Request;
			if (
				(request.Method == HttpMethods.Post || request.Method == HttpMethods.Put) 
				&& request.ContentLength > 0)
			{
				request.EnableBuffering();
				var buffer = new byte[Convert.ToInt32(request.ContentLength)];
				await request.Body.ReadAsync(buffer, 0, buffer.Length);
				var requestContent = Encoding.UTF8.GetString(buffer);
				request.Body.Position = 0;
				log.Body = requestContent;
			}
			try
			{
				var service = httpContext.RequestServices.GetService<IHTTPLogService>();
				var response = service.Add(log);
				if(response.Data == null || response.StatusCode != HttpStatusCode.OK)
				{
					Console.WriteLine("Log Failed.");
				}
				else
				{
					Console.WriteLine($@"
================================
Status: {log.StatusCode}
Date: {log.Date}
Type: {log.Type}
Request: {log.Request}
Body: {log.Body}
================================");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			await _next(httpContext);

		}
	}
}
