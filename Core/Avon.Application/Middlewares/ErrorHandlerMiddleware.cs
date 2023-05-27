using Avon.Application.Exceptions;
using Avon.Application.Exceptions.BadRequestExceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Avon.Application.Middlewares
{
    public class ErrorHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ContentTypeException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case PromotionNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ProductNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case UserNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

    }
}
