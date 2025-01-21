using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Util.Error
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (YapidromException ex)
            {
                await HandleCustomExceptionAsync(context, ex);
            }
            catch (Exception)
            {
                await HandleDefaultExceptionAsync(context);
            }
        }

        private Task HandleCustomExceptionAsync(HttpContext context, YapidromException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.HttpStatusCode;

            var response = new
            {
                Success = false,
                ErrorCode = ex.Code,
                Message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleDefaultExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                Success = false,
                ErrorCode = ErrorCodes.SystemFailure.Code,
                Message = ErrorCodes.SystemFailure.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
