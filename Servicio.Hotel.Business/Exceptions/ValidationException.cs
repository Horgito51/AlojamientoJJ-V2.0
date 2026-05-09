using System;
using System.Collections.Generic;

namespace Servicio.Hotel.Business.Exceptions
{
    public class ValidationException : Exception
    {
        public string? CodigoError { get; set; }
        public IDictionary<string, string[]> Errors { get; private set; } = new Dictionary<string, string[]>();

        public ValidationException() : base() { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }

        public ValidationException(string codigoError, string message) : base(message)
        {
            CodigoError = codigoError;
        }

        public ValidationException(string codigoError, string message, Exception innerException) : base(message, innerException)
        {
            CodigoError = codigoError;
        }

        // 👇 Este es el constructor que estabas usando en los validadores
        public ValidationException(string codigoError, IDictionary<string, string[]> errors)
            : base("Se produjeron uno o más errores de validación.")
        {
            CodigoError = codigoError;
            Errors = errors;
        }

        public ValidationException(string codigoError, string message, IDictionary<string, string[]> errors)
            : base(message)
        {
            CodigoError = codigoError;
            Errors = errors;
        }
    }
}
