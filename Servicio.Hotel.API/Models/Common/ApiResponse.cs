namespace Servicio.Hotel.API.Models.Common
{
    // Versión genérica
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message, int statusCode, bool success = true)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        // Métodos factory renombrados
        public static ApiResponse<T> Ok(T data, string message = "Operación exitosa")
            => new ApiResponse<T>(data, message, 200);

        public static ApiResponse<T> Created(T data, string message = "Recurso creado")
            => new ApiResponse<T>(data, message, 201);

        public static ApiResponse<T> NoContent(string message = "Sin contenido")
            => new ApiResponse<T>(default!, message, 204);
    }

    // Versión no genérica
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, string message, int statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public static ApiResponse Ok(string message = "Operación exitosa")
            => new ApiResponse(true, message, 200);

        public static ApiResponse Created(string message = "Recurso creado")
            => new ApiResponse(true, message, 201);

        public static ApiResponse NoContent(string message = "Sin contenido")
            => new ApiResponse(true, message, 204);
    }
}