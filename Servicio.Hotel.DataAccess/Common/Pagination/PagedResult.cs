using System.Collections.Generic;

namespace Servicio.Hotel.DataAccess.Common.Pagination
{
    /// <summary>
    /// Representa un resultado paginado de una consulta.
    /// </summary>
    /// <typeparam name="T">Tipo de elementos en la página.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Elementos de la página actual.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Número total de registros que cumplen los criterios de búsqueda.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Número de página actual (1-indexed).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Cantidad de elementos por página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Cantidad total de páginas.
        /// </summary>
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

        /// <summary>
        /// Indica si existe una página anterior.
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Indica si existe una página siguiente.
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;
    }
}