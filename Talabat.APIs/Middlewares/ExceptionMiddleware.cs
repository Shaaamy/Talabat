﻿using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //Production => log error in database
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var Response = _env.IsDevelopment() ? new ApiExceptionResponse(500, ex.Message, ex.StackTrace) : new ApiExceptionResponse(500);
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase

                };
                var JsonResponse = JsonSerializer.Serialize(Response, Options);
                await context.Response.WriteAsync(JsonResponse);
            }

        }
    }
}