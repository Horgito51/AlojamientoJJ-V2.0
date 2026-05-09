using System;

namespace Servicio.Hotel.Business.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando ocurre un error de negocio (reglas de negocio, operaciones inválidas, etc.)
    /// </summary>
    public class BusinessException : Exception
    {
        public string? CodigoError { get; set; }

        public BusinessException() : base() { }

        public BusinessException(string message) : base(message) { }

        public BusinessException(string codigoError, string message) : base(message)
        {
            CodigoError = codigoError;
        }

        public BusinessException(string message, Exception innerException)
            : base(message, innerException) { }

        public BusinessException(string codigoError, string message, Exception innerException)
            : base(message, innerException)
        {
            CodigoError = codigoError;
        }
    }
}
