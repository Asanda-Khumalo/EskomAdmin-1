using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using EskomAdmin.Server.Data;

namespace EskomAdmin.Server
{
    public partial class DeshboardService
    {
        DeshboardContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly DeshboardContext context;
        private readonly NavigationManager navigationManager;

        public DeshboardService(DeshboardContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportDeshboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/deshboard/deshboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/deshboard/deshboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDeshboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/deshboard/deshboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/deshboard/deshboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDeshboardsRead(ref IQueryable<EskomAdmin.Server.Models.Deshboard.Deshboard> items);

        public async Task<IQueryable<EskomAdmin.Server.Models.Deshboard.Deshboard>> GetDeshboards(Query query = null)
        {
            var items = Context.Deshboards.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDeshboardsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDeshboardGet(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnGetDeshboardByTrendNumber(ref IQueryable<EskomAdmin.Server.Models.Deshboard.Deshboard> items);


        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> GetDeshboardByTrendNumber(int trendnumber)
        {
            var items = Context.Deshboards
                              .AsNoTracking()
                              .Where(i => i.TrendNumber == trendnumber);

 
            OnGetDeshboardByTrendNumber(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDeshboardGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDeshboardCreated(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardCreated(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> CreateDeshboard(EskomAdmin.Server.Models.Deshboard.Deshboard deshboard)
        {
            OnDeshboardCreated(deshboard);

            var existingItem = Context.Deshboards
                              .Where(i => i.TrendNumber == deshboard.TrendNumber)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Deshboards.Add(deshboard);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(deshboard).State = EntityState.Detached;
                throw;
            }

            OnAfterDeshboardCreated(deshboard);

            return deshboard;
        }

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> CancelDeshboardChanges(EskomAdmin.Server.Models.Deshboard.Deshboard item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDeshboardUpdated(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardUpdated(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> UpdateDeshboard(int trendnumber, EskomAdmin.Server.Models.Deshboard.Deshboard deshboard)
        {
            OnDeshboardUpdated(deshboard);

            var itemToUpdate = Context.Deshboards
                              .Where(i => i.TrendNumber == deshboard.TrendNumber)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(deshboard);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDeshboardUpdated(deshboard);

            return deshboard;
        }

        partial void OnDeshboardDeleted(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardDeleted(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        public async Task<EskomAdmin.Server.Models.Deshboard.Deshboard> DeleteDeshboard(int trendnumber)
        {
            var itemToDelete = Context.Deshboards
                              .Where(i => i.TrendNumber == trendnumber)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDeshboardDeleted(itemToDelete);


            Context.Deshboards.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDeshboardDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}