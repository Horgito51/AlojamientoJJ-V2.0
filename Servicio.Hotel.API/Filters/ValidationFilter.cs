using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Servicio.Hotel.API.Models.Common;
using System.Linq;

namespace Servicio.Hotel.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Extraer errores en un formato amigable
                var errors = context.ModelState
                    .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var response = new ApiErrorResponse(
                    message: "Uno o más errores de validación ocurrieron.",
                    statusCode: 400,
                    errors: errors,
                    traceId: context.HttpContext.TraceIdentifier
                );

                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No se requiere lógica después de ejecutar la acción
        }
    }
}
