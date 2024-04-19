using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using DeanRemoteMonitoringWeb.Server.Data;

namespace DeanRemoteMonitoringWeb.Server.Controllers
{
    public partial class ExportRAZDENController : ExportController
    {
        private readonly RAZDENContext context;
        private readonly RAZDENService service;

        public ExportRAZDENController(RAZDENContext context, RAZDENService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/RAZDEN/fuelrefillings/csv")]
        [HttpGet("/export/RAZDEN/fuelrefillings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFuelRefillingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFuelRefillings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/RAZDEN/fuelrefillings/excel")]
        [HttpGet("/export/RAZDEN/fuelrefillings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFuelRefillingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFuelRefillings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/RAZDEN/fueltanks/csv")]
        [HttpGet("/export/RAZDEN/fueltanks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFuelTanksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFuelTanks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/RAZDEN/fueltanks/excel")]
        [HttpGet("/export/RAZDEN/fueltanks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFuelTanksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFuelTanks(), Request.Query, false), fileName);
        }
    }
}
