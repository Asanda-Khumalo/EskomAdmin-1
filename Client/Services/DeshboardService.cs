
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

namespace EskomAdmin.Client
{
    public partial class DeshboardService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public DeshboardService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Deshboard/");
        }


        public async System.Threading.Tasks.Task ExportDeshboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/deshboard/deshboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/deshboard/deshboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportDeshboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/deshboard/deshboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/deshboard/deshboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetDeshboards(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<EskomAdmin.Server.Models.Deshboard.Deshboard>> GetDeshboards(Query query)
        {
            return await GetDeshboards(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<EskomAdmin.Server.Models.Deshboard.Deshboard>> GetDeshboards(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string), string apply = default(string))
        {
            var uri = new Uri(baseUri, $"Deshboards");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count, apply:apply);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDeshboards(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<EskomAdmin.Server.Models.Deshboard.Deshboard>>(response);
        }

        partial void OnCreateDeshboard(HttpRequestMessage requestMessage);

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> CreateDeshboard(EskomAdmin.Server.Models.Deshboard.Deshboard deshboard = default(EskomAdmin.Server.Models.Deshboard.Deshboard))
        {
            var uri = new Uri(baseUri, $"Deshboards");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(deshboard), Encoding.UTF8, "application/json");

            OnCreateDeshboard(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EskomAdmin.Server.Models.Deshboard.Deshboard>(response);
        }

        partial void OnDeleteDeshboard(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteDeshboard(int trendNumber = default(int))
        {
            var uri = new Uri(baseUri, $"Deshboards({trendNumber})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteDeshboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetDeshboardByTrendNumber(HttpRequestMessage requestMessage);

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> GetDeshboardByTrendNumber(string expand = default(string), int trendNumber = default(int))
        {
            var uri = new Uri(baseUri, $"Deshboards({trendNumber})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDeshboardByTrendNumber(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<EskomAdmin.Server.Models.Deshboard.Deshboard>(response);
        }

        partial void OnUpdateDeshboard(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateDeshboard(int trendNumber = default(int), EskomAdmin.Server.Models.Deshboard.Deshboard deshboard = default(EskomAdmin.Server.Models.Deshboard.Deshboard))
        {
            var uri = new Uri(baseUri, $"Deshboards({trendNumber})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", deshboard.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(deshboard), Encoding.UTF8, "application/json");

            OnUpdateDeshboard(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}