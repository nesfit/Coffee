using System;

namespace Barista.Common.Dto
{
    public class ErrorDto
    {
        public ErrorDto(string code, string message)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));

            Code = code;
            Message = message;
        }

        public string Code { get; }
        public string Message { get; }
    }
}
