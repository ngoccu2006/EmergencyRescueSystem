using System;
using System.Collections.Generic;
using System.Text;

namespace RescueSystem.Application.Common.Exception
{
    internal class BadRequestException : System.Exception
    {
        public int StatusCode { get; } = 400;
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
