using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using EskomAdmin.Server.Data;

namespace EskomAdmin.Server.Controllers
{
    public partial class ExportDeshboardController : ExportController
    {
        private readonly DeshboardContext context;
        private readonly DeshboardService service;

        public ExportDeshboardController(DeshboardContext context, DeshboardService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Deshboard/deshboards/csv")]
        [HttpGet("/export/Deshboard/deshboards/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDeshboardsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDeshboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Deshboard/deshboards/excel")]
        [HttpGet("/export/Deshboard/deshboards/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDeshboardsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDeshboards(), Request.Query, false), fileName);
        }
    }
}
