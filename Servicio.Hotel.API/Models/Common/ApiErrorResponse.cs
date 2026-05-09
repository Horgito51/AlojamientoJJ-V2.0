namespace Servicio.Hotel.API.Models.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
        public string? TraceId { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiErrorResponse() => Timestamp = DateTime.UtcNow;

        public ApiErrorResponse(string message, int statusCode, Dictionary<string, string[]>? errors = null, string? traceId = null)
        {
            Success = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
            TraceId = traceId;
            Timestamp = DateTime.UtcNow;
        }

        public static ApiErrorResponse BadRequest(string message = "Solicitud inválida", Dictionary<string, string[]>? errors = null, string? traceId = null)
            => new ApiErrorResponse(message, 400, errors, traceId);

        public static ApiErrorResponse Unauthorized(string message = "No autorizado", string? traceId = null)
            => new ApiErrorResponse(message, 401, null, traceId);

        public static ApiErrorResponse Forbidden(string message = "Acceso denegado", string? traceId = null)
            => new ApiErrorResponse(message, 403, null, traceId);

        public static ApiErrorResponse NotFound(string message = "Recurso no encontrado", string? traceId = null)
            => new ApiErrorResponse(message, 404, null, traceId);

        public static ApiErrorResponse Conflict(string message = "Conflicto con el estado actual", string? traceId = null)
            => new ApiErrorResponse(message, 409, null, traceId);

        public static ApiErrorResponse InternalError(string message = "Error interno del servidor", string? traceId = null)
            => new ApiErrorResponse(message, 500, null, traceId);
    }
}