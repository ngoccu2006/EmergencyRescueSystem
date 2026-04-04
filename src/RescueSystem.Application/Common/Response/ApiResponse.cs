using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RescueSystem.Application.Common.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Request successful", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> ErrorResponse(string message = "Internal Server Error", int statusCode = 500, object? errors = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
        }
    }
}
