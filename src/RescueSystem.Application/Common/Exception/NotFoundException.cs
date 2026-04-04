using System;
using System.Collections.Generic;
using System.Text;

namespace RescueSystem.Application.Common.Exception
{
    internal class NotFoundException : System.Exception
    {
        public int StatusCode { get; } = 404;
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
