using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BLL.Interfaces;
using WebApi.DAL.Entities;
using WebApi.DAL.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ExportDataController : ControllerBase
    {
        private readonly IUnitOfWork database;
        private readonly IExportService exportService;

        public ExportDataController(IUnitOfWork database, IExportService exportService)
        {
            this.database = database;
            this.exportService = exportService;
        }

        /// <summary>
        /// Экспорт данных в CSV-файл
        /// </summary>
        /// <param name="fromDt">Дата создания От</param>
        /// <param name="toDt">Дата создания По</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExportToCsv(DateTime fromDt, DateTime toDt)
        {
            if (fromDt >= toDt)
            {
                return BadRequest();
            }

            try
            {
                var data = await database.GetRepository<TestModel>()
                    .Find(x => !x.IsDeleted && fromDt <= x.CreatedAt && x.CreatedAt <= toDt)
                    .OrderBy(x => x.CreatedAt)
                    .ToListAsync();

                var exportResult = await exportService.ExportDataAsync(data, excludeColumnNames: new[] { "ID" });
                var stream = new MemoryStream(Encoding.Default.GetBytes(exportResult));
                var fileResult = new FileStreamResult(stream, "text/plain")
                {
                    FileDownloadName = $"export_from_{fromDt.ToShortDateString()}_to_{toDt.ToShortDateString()}.csv"
                };

                return fileResult;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
