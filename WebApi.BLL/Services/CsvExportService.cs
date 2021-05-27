using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Interfaces;

namespace WebApi.BLL.Services
{
    public class CsvExportService : IExportService
    {
        public Task<string> ExportDataAsync<T>(IEnumerable<T> source, string[] excludeColumnNames = null)
        {
            var properties = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var columnNames = properties
                .Where(x => excludeColumnNames == null || !excludeColumnNames.Contains(x.Name))
                .Select(x => x.Name);
            var csv = new StringBuilder();
            csv.AppendJoin(";", columnNames);
            csv.AppendLine();

            foreach (var obj in source)
            {
                csv.AppendJoin(";", obj.ToString());
                csv.AppendLine();
            }

            return Task.FromResult(csv.ToString());
        }
    }
}
