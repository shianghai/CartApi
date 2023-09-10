using CartApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartApi.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly RequestDelegate _next;
        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BackendException e)
            {
                _logger.LogError(e.Message, e);

                context.Response.StatusCode = e.Code;

                string response = new Response<object>
                {
                    Message = e.Message,
                    Status = false,
                    Data = new List<object> { e.Data }
                }.ToString();

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                string response = new Response<object>
                {
                    Message = e.Message,
                    Status = false,
                    Data = new List<object> { e.Data }
                }.ToString();

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(response);


            }
           
        }
    }
}
