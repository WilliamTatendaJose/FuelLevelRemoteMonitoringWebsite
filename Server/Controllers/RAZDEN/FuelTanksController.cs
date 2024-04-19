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
    [Route("odata/RAZDEN/FuelTanks")]
    public partial class FuelTanksController : ODataController
    {
        private DeanRemoteMonitoringWeb.Server.Data.RAZDENContext context;

        public FuelTanksController(DeanRemoteMonitoringWeb.Server.Data.RAZDENContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> GetFuelTanks()
        {
            var items = this.context.FuelTanks.AsQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank>();
            this.OnFuelTanksRead(ref items);

            return items;
        }

        partial void OnFuelTanksRead(ref IQueryable<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> items);

        partial void OnFuelTankGet(ref SingleResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/RAZDEN/FuelTanks(Tank={Tank})")]
        public SingleResult<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> GetFuelTank(int key)
        {
            var items = this.context.FuelTanks.Where(i => i.Tank == key);
            var result = SingleResult.Create(items);

            OnFuelTankGet(ref result);

            return result;
        }
        partial void OnFuelTankDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankDeleted(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        [HttpDelete("/odata/RAZDEN/FuelTanks(Tank={Tank})")]
        public IActionResult DeleteFuelTank(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.FuelTanks
                    .Where(i => i.Tank == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFuelTankDeleted(item);
                this.context.FuelTanks.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFuelTankDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFuelTankUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankUpdated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        [HttpPut("/odata/RAZDEN/FuelTanks(Tank={Tank})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutFuelTank(int key, [FromBody]DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Tank != key))
                {
                    return BadRequest();
                }
                this.OnFuelTankUpdated(item);
                this.context.FuelTanks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelTanks.Where(i => i.Tank == key);
                
                this.OnAfterFuelTankUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/RAZDEN/FuelTanks(Tank={Tank})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchFuelTank(int key, [FromBody]Delta<DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.FuelTanks.Where(i => i.Tank == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFuelTankUpdated(item);
                this.context.FuelTanks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelTanks.Where(i => i.Tank == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFuelTankCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);
        partial void OnAfterFuelTankCreated(DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DeanRemoteMonitoringWeb.Server.Models.RAZDEN.FuelTank item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnFuelTankCreated(item);
                this.context.FuelTanks.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.FuelTanks.Where(i => i.Tank == item.Tank);

                

                this.OnAfterFuelTankCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
