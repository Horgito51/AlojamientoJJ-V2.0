using System;

namespace Servicio.Hotel.DataManagement.Exceptions
{
    /// <summary>
    /// Excepción de reglas de dominio que deben mapearse a 422 Unprocessable Entity.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}

