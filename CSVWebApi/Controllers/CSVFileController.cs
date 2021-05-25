using CSVWebApi.EF;
using CSVWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CSVFileController : ControllerBase
    {
        private readonly TestDbContext context;

        public CSVFileController(TestDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Экспорт данных в CSV-файл
        /// </summary>
        /// <param name="fromDt">Дата создания От</param>
        /// <param name="toDt">Дата создания По</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Export(DateTime fromDt, DateTime toDt)
        {
            if (fromDt >= toDt)
            {
                return BadRequest();
            }

            try
            {
                var csv = await GetDataFromCsvAsync(fromDt, toDt);            
                var stream = new MemoryStream(Encoding.Default.GetBytes(csv));
                var result = new FileStreamResult(stream, "text/plain")
                {
                    FileDownloadName = $"export_from_{fromDt.ToShortDateString()}_to_{toDt.ToShortDateString()}.csv"
                };

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        private async Task<string> GetDataFromCsvAsync(DateTime fromDt, DateTime toDt)
        {
            var data = await context.TestModels
                .Where(x => !x.IsDeleted && fromDt <= x.CreatedAt && x.CreatedAt <= toDt)
                .OrderBy(x => x.CreatedAt)
                .Select(x => new object[]
                {
                    x.Name,
                    x.Description,
                    x.CreatedAt,
                    x.EditedAt
                })
                .ToListAsync();

            var columnNames = TestModel.GetColumnNames();
            var csv = new StringBuilder();
            csv.AppendJoin(";", columnNames);
            csv.AppendLine();

            foreach (var obj in data)
            {
                csv.AppendJoin(";", obj);
                csv.AppendLine();
            }

            return csv.ToString();
        }
    }
}
