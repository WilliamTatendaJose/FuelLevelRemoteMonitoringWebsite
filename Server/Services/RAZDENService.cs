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

using DeanRemoteMonitoringWeb.Server.Data;

namespace DeanRemoteMonitoringWeb.Server
{
    public partial class RAZDENService
    {
        RAZDENContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly RAZDENContext context;
        private readonly NavigationManager navigationManager;

        public RAZDENService(RAZDENContext context, NavigationManager navigationManager)
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


        public async Task ExportFuelRefillingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fuelrefillings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fuelrefillings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFuelRefillingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fuelrefillings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fuelrefillings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFuelRefillingsRead(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> items);

        public async Task<IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>> GetFuelRefillings(Query query = null)
        {
            var items = Context.FuelRefillings.AsQueryable();


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

            OnFuelRefillingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFuelRefillingGet(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnGetFuelRefillingByTank(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> items);


        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> GetFuelRefillingByTank(int tank)
        {
            var items = Context.FuelRefillings
                              .AsNoTracking()
                              .Where(i => i.Tank == tank);


            OnGetFuelRefillingByTank(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFuelRefillingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFuelRefillingCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> CreateFuelRefilling(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling fueltank)
        {
            OnFuelRefillingCreated(fueltank);

            var existingItem = Context.FuelRefillings
                              .Where(i => i.Tank == fueltank.Tank)
                              .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.FuelRefillings.Add(fueltank);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fueltank).State = EntityState.Detached;
                throw;
            }

            OnAfterFuelRefillingCreated(fueltank);

            return fueltank;
        }

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> CancelFuelRefillingChanges(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFuelRefillingUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> UpdateFuelRefilling(int tank, DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling fueltank)
        {
            OnFuelRefillingUpdated(fueltank);

            var itemToUpdate = Context.FuelRefillings
                              .Where(i => i.Tank == fueltank.Tank)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fueltank);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFuelRefillingUpdated(fueltank);

            return fueltank;
        }

        partial void OnFuelRefillingDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> DeleteFuelRefilling(int tank)
        {
            var itemToDelete = Context.FuelRefillings
                              .Where(i => i.Tank == tank)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnFuelRefillingDeleted(itemToDelete);


            Context.FuelRefillings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFuelRefillingDeleted(itemToDelete);

            return itemToDelete;
        }


        public async Task ExportFuelTanksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fueltanks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fueltanks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFuelTanksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/razden/fueltanks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/razden/fueltanks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFuelTanksRead(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> items);

        public async Task<IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>> GetFuelTanks(Query query = null)
        {
            var items = Context.FuelTanks.AsQueryable();


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

            OnFuelTanksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFuelTankGet(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnGetFuelTankByTank(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> items);


        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> GetFuelTankByTank(int tank)
        {
            var items = Context.FuelTanks
                              .AsNoTracking()
                              .Where(i => i.Tank == tank);

 
            OnGetFuelTankByTank(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFuelTankGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFuelTankCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> CreateFuelTank(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank fueltank)
        {
            OnFuelTankCreated(fueltank);

            var existingItem = Context.FuelTanks
                              .Where(i => i.Tank == fueltank.Tank)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.FuelTanks.Add(fueltank);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fueltank).State = EntityState.Detached;
                throw;
            }

            OnAfterFuelTankCreated(fueltank);

            return fueltank;
        }

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> CancelFuelTankChanges(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFuelTankUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> UpdateFuelTank(int tank, DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank fueltank)
        {
            OnFuelTankUpdated(fueltank);

            var itemToUpdate = Context.FuelTanks
                              .Where(i => i.Tank == fueltank.Tank)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fueltank);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFuelTankUpdated(fueltank);

            return fueltank;
        }

        partial void OnFuelTankDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        public async Task<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> DeleteFuelTank(int tank)
        {
            var itemToDelete = Context.FuelTanks
                              .Where(i => i.Tank == tank)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFuelTankDeleted(itemToDelete);


            Context.FuelTanks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFuelTankDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}