using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DeanRemoteMonitoringWeb.Server.Controllers.RAZDEN
{
    [Route("odata/RAZDEN/FuelRefillings")]
    public partial class FuelRefillingsController : ODataController
    {
        private DeanRemoteMonitoringWeb.Server.Data.RAZDENContext context;

        public FuelRefillingsController(DeanRemoteMonitoringWeb.Server.Data.RAZDENContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IEnumerable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> GetFuelRefillings()
        {
            var items = this.context.FuelRefillings.AsQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling>();
            this.OnFuelRefillingsRead(ref items);

            return items;
        }

        partial void OnFuelRefillingsRead(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> items);

        partial void OnFuelRefillingGet(ref SingleResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> item);

        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        [HttpGet("/odata/RAZDEN/FuelRefillings(Tank={Tank})")]
        public SingleResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> GetFuelRefilling(int key)
        {
            var items = this.context.FuelRefillings.Where(i => i.Tank == key);
            var result = SingleResult.Create(items);

            OnFuelRefillingGet(ref result);

            return result;
        }
        partial void OnFuelRefillingDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        [HttpDelete("/odata/RAZDEN/FuelRefillings(Tank={Tank})")]
        public IActionResult DeleteFuelRefilling(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.FuelRefillings
                    .Where(i => i.Tank == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFuelRefillingDeleted(item);
                this.context.FuelRefillings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFuelRefillingDeleted(item);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFuelRefillingUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        [HttpPut("/odata/RAZDEN/FuelRefillings(Tank={Tank})")]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult PutFuelRefilling(int key, [FromBody] DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Tank != key))
                {
                    return BadRequest();
                }
                this.OnFuelRefillingUpdated(item);
                this.context.FuelRefillings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelRefillings.Where(i => i.Tank == key);

                this.OnAfterFuelRefillingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/RAZDEN/FuelRefillings(Tank={Tank})")]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult PatchFuelRefilling(int key, [FromBody] Delta<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling> patch)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.FuelRefillings.Where(i => i.Tank == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFuelRefillingUpdated(item);
                this.context.FuelRefillings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelRefillings.Where(i => i.Tank == key);

                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFuelRefillingCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);
        partial void OnAfterFuelRefillingCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth = 10, MaxAnyAllExpressionDepth = 10, MaxNodeCount = 1000)]
        public IActionResult Post([FromBody] DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelRefilling item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnFuelRefillingCreated(item);
                this.context.FuelRefillings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelRefillings.Where(i => i.Tank == item.Tank);



                this.OnAfterFuelRefillingCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
