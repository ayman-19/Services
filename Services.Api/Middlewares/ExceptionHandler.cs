﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Services.Shared.Exceptions;
using Services.Shared.Responses;

namespace Services.Api.Middlewares
{
    public sealed class ExceptionHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var requestStatus = GetStatusRequest(ex);
                var resposne = new Response
                {
                    Message = requestStatus.message.Replace('/', ','),
                    StatusCode = requestStatus.statusCode,
                    Success = false,
                };
                context.Response.StatusCode = requestStatus.statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposne));
            }
        }

        private (int statusCode, string message) GetStatusRequest(Exception ex) =>
            ex switch
            {
                ValidationException => ((int)HttpStatusCode.BadRequest, ex.Message),
                InvalidException => ((int)HttpStatusCode.BadRequest, ex.Message),
                DatabaseTransactionException => ((int)HttpStatusCode.Conflict, ex.Message),
                NotSupportedException => (501, ex.Message),
                UnauthorizedAccessException => ((int)HttpStatusCode.Unauthorized, ex.Message),
                NotConfirmEmail => (499, ex.Message),
                _ => ((int)HttpStatusCode.InternalServerError, ex.Message),
            };
    }
}
