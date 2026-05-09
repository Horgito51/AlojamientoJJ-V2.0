using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Servicio.Hotel.Business.DTOs.Reservas;
using Servicio.Hotel.Business.Exceptions;

namespace Servicio.Hotel.Business.Validators.Reservas
{
    public static class ClienteValidator
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex Digits10Regex = new(@"^\d{10}$", RegexOptions.Compiled);
        private static readonly Regex Digits13Regex = new(@"^\d{13}$", RegexOptions.Compiled);

        public static void Validate(ClienteDTO cliente)
        {
            if (cliente == null)
                throw new ValidationException("CLI-001", "El cliente no puede ser nulo.");

            var errors = new Dictionary<string, string[]>();
            var tipo = (cliente.TipoIdentificacion ?? string.Empty).Trim().ToUpperInvariant();
            var identificacion = OnlyDigits(cliente.NumeroIdentificacion);
            var correo = (cliente.Correo ?? string.Empty).Trim();
            var telefono = OnlyDigits(cliente.Telefono);

            if (string.IsNullOrWhiteSpace(tipo))
                errors["TipoIdentificacion"] = new[] { "El tipo de identificacion es obligatorio." };

            if (string.IsNullOrWhiteSpace(identificacion))
                errors["NumeroIdentificacion"] = new[] { "El numero de identificacion es obligatorio." };
            else if (EsTipoCedula(tipo) && !EsCedulaValida(identificacion))
                errors["NumeroIdentificacion"] = new[] { "La cedula ecuatoriana no es valida." };
            else if (EsTipoRuc(tipo) && !EsRucValido(identificacion))
                errors["NumeroIdentificacion"] = new[] { "El RUC ecuatoriano no es valido." };
            else if (!EsTipoCedula(tipo) && !EsTipoRuc(tipo) && !Digits10Regex.IsMatch(identificacion) && !Digits13Regex.IsMatch(identificacion))
                errors["NumeroIdentificacion"] = new[] { "La identificacion debe tener 10 o 13 digitos." };

            if (string.IsNullOrWhiteSpace(cliente.Nombres) && string.IsNullOrWhiteSpace(cliente.RazonSocial))
                errors["Nombres"] = new[] { "Los nombres o razon social son obligatorios." };

            if (string.IsNullOrWhiteSpace(correo))
                errors["Correo"] = new[] { "El correo electronico es obligatorio." };
            else if (!EmailRegex.IsMatch(correo))
                errors["Correo"] = new[] { "El correo electronico no tiene un formato valido." };

            if (string.IsNullOrWhiteSpace(telefono))
                errors["Telefono"] = new[] { "El telefono es obligatorio." };
            else if (!Digits10Regex.IsMatch(telefono))
                errors["Telefono"] = new[] { "El telefono debe contener exactamente 10 digitos." };

            if (errors.Count > 0)
                throw new ValidationException("CLI-002", errors);
        }

        private static bool EsTipoCedula(string tipo)
            => tipo is "CED" or "CEDULA" or "C" or "CLI";

        private static bool EsTipoRuc(string tipo)
            => tipo is "RUC" or "R";

        private static string OnlyDigits(string? value)
            => new((value ?? string.Empty).Where(char.IsDigit).ToArray());

        private static bool EsCedulaValida(string cedula)
        {
            if (!Digits10Regex.IsMatch(cedula)) return false;

            var provincia = int.Parse(cedula[..2]);
            if (provincia < 1 || provincia > 24) return false;

            var tercerDigito = cedula[2] - '0';
            if (tercerDigito > 5) return false;

            var suma = 0;
            for (var i = 0; i < 9; i++)
            {
                var valor = cedula[i] - '0';
                if (i % 2 == 0)
                {
                    valor *= 2;
                    if (valor > 9) valor -= 9;
                }
                suma += valor;
            }

            var digitoVerificador = (10 - (suma % 10)) % 10;
            return digitoVerificador == cedula[9] - '0';
        }

        private static bool EsRucValido(string ruc)
        {
            if (!Digits13Regex.IsMatch(ruc)) return false;
            if (ruc.Substring(10, 3) == "000") return false;

            var tercerDigito = ruc[2] - '0';
            if (tercerDigito < 6)
                return EsCedulaValida(ruc[..10]);

            return tercerDigito == 6
                ? EsRucPublicoValido(ruc)
                : tercerDigito == 9 && EsRucPrivadoValido(ruc);
        }

        private static bool EsRucPrivadoValido(string ruc)
        {
            var coeficientes = new[] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            var suma = coeficientes.Select((c, i) => c * (ruc[i] - '0')).Sum();
            var mod = suma % 11;
            var digito = mod == 0 ? 0 : 11 - mod;
            return digito == ruc[9] - '0';
        }

        private static bool EsRucPublicoValido(string ruc)
        {
            var coeficientes = new[] { 3, 2, 7, 6, 5, 4, 3, 2 };
            var suma = coeficientes.Select((c, i) => c * (ruc[i] - '0')).Sum();
            var mod = suma % 11;
            var digito = mod == 0 ? 0 : 11 - mod;
            return digito == ruc[8] - '0';
        }
    }
}
