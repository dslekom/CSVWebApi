using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.BLL.Interfaces
{
    public interface IExportService
    {
        /// <summary>
        /// Формирование строки из данных для экспорта
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Список данных</param>
        /// <param name="excludeColumnNames">Список свойств для исключения в выборку</param>
        /// <returns></returns>
        Task<string> ExportDataAsync<T>(IEnumerable<T> source, string[] excludeColumnNames = null);
    }
}
