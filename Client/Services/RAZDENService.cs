
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace DeanRemoteMonitoringWeb.Client
{
    public partial class RAZDENService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public RAZDENService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/RAZDEN/");
        }


        public async System.Threading.Tasks.Task ExportFuelRefillingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fuelrefillings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fuelrefillings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportFuelRefillingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fuelrefillings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fuelrefillings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetFuelRefillings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>> GetFuelRefillings(Query query)
        {
            return await GetFuelRefillings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>> GetFuelRefillings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"FuelRefillings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFuelRefillings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>>(response);
        }
        partial void OnCreateFuelRefilling(HttpRequestMessage requestMessage);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> CreateFuelRefilling(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling fuelTank = default(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling))
        {
            var uri = new Uri(baseUri, $"FuelRefillings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(fuelTank), Encoding.UTF8, "application/json");

            OnCreateFuelRefilling(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>(response);
        }

        partial void OnDeleteFuelRefilling(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteFuelRefilling(int tank = default(int))
        {
            var uri = new Uri(baseUri, $"FuelRefillings({tank})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteFuelRefilling(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetFuelRefillingByTank(HttpRequestMessage requestMessage);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> GetFuelRefillingByTank(string expand = default(string), int tank = default(int))
        {
            var uri = new Uri(baseUri, $"FuelRefillings({tank})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter: null, top: null, skip: null, orderby: null, expand: expand, select: null, count: null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFuelRefillingByTank(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>(response);
        }

        partial void OnUpdateFuelRefilling(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> UpdateFuelRefilling(int tank = default(int), DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling fuelTank = default(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling))
        {
            var uri = new Uri(baseUri, $"FuelRefillings({tank})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(fuelTank), Encoding.UTF8, "application/json");

            OnUpdateFuelRefilling(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportFuelTanksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fueltanks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fueltanks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportFuelTanksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fueltanks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fueltanks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetFuelTanks(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>> GetFuelTanks(Query query)
        {
            return await GetFuelTanks(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>> GetFuelTanks(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"FuelTanks");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFuelTanks(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>>(response);
        }

        partial void OnCreateFuelTank(HttpRequestMessage requestMessage);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> CreateFuelTank(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank fuelTank = default(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank))
        {
            var uri = new Uri(baseUri, $"FuelTanks");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(fuelTank), Encoding.UTF8, "application/json");

            OnCreateFuelTank(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>(response);
        }

        partial void OnDeleteFuelTank(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteFuelTank(int tank = default(int))
        {
            var uri = new Uri(baseUri, $"FuelTanks({tank})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteFuelTank(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetFuelTankByTank(HttpRequestMessage requestMessage);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> GetFuelTankByTank(string expand = default(string), int tank = default(int))
        {
            var uri = new Uri(baseUri, $"FuelTanks({tank})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetFuelTankByTank(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>(response);
        }

        partial void OnUpdateFuelTank(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateFuelTank(int tank = default(int), DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank fuelTank = default(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank))
        {
            var uri = new Uri(baseUri, $"FuelTanks({tank})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(fuelTank), Encoding.UTF8, "application/json");

            OnUpdateFuelTank(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}