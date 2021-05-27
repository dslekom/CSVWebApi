using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.BLL.Interfaces
{
    public interface IExportService
    {
        Task<string> ExportDataAsync<T>(IEnumerable<T> source, string[] excludeColumnNames = null);
    }
}
