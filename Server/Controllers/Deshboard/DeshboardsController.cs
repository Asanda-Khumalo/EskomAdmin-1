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

namespace EskomAdmin.Server.Controllers.Deshboard
{
    [Route("odata/Deshboard/Deshboards")]
    public partial class DeshboardsController : ODataController
    {
        private EskomAdmin.Server.Data.DeshboardContext context;

        public DeshboardsController(EskomAdmin.Server.Data.DeshboardContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<EskomAdmin.Server.Models.Deshboard.Deshboard> GetDeshboards()
        {
            var items = this.context.Deshboards.AsQueryable<EskomAdmin.Server.Models.Deshboard.Deshboard>();
            this.OnDeshboardsRead(ref items);

            return items;
        }

        partial void OnDeshboardsRead(ref IQueryable<EskomAdmin.Server.Models.Deshboard.Deshboard> items);

        partial void OnDeshboardGet(ref SingleResult<EskomAdmin.Server.Models.Deshboard.Deshboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Deshboard/Deshboards(TrendNumber={TrendNumber})")]
        public SingleResult<EskomAdmin.Server.Models.Deshboard.Deshboard> GetDeshboard(int key)
        {
            var items = this.context.Deshboards.Where(i => i.TrendNumber == key);
            var result = SingleResult.Create(items);

            OnDeshboardGet(ref result);

            return result;
        }
        partial void OnDeshboardDeleted(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardDeleted(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        [HttpDelete("/odata/Deshboard/Deshboards(TrendNumber={TrendNumber})")]
        public IActionResult DeleteDeshboard(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Deshboards
                    .Where(i => i.TrendNumber == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<EskomAdmin.Server.Models.Deshboard.Deshboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDeshboardDeleted(item);
                this.context.Deshboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDeshboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDeshboardUpdated(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardUpdated(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        [HttpPut("/odata/Deshboard/Deshboards(TrendNumber={TrendNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDeshboard(int key, [FromBody]EskomAdmin.Server.Models.Deshboard.Deshboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Deshboards
                    .Where(i => i.TrendNumber == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<EskomAdmin.Server.Models.Deshboard.Deshboard>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDeshboardUpdated(item);
                this.context.Deshboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Deshboards.Where(i => i.TrendNumber == key);
                
                this.OnAfterDeshboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Deshboard/Deshboards(TrendNumber={TrendNumber})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDeshboard(int key, [FromBody]Delta<EskomAdmin.Server.Models.Deshboard.Deshboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Deshboards
                    .Where(i => i.TrendNumber == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<EskomAdmin.Server.Models.Deshboard.Deshboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnDeshboardUpdated(item);
                this.context.Deshboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Deshboards.Where(i => i.TrendNumber == key);
                
                this.OnAfterDeshboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDeshboardCreated(EskomAdmin.Server.Models.Deshboard.Deshboard item);
        partial void OnAfterDeshboardCreated(EskomAdmin.Server.Models.Deshboard.Deshboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] EskomAdmin.Server.Models.Deshboard.Deshboard item)
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

                this.OnDeshboardCreated(item);
                this.context.Deshboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Deshboards.Where(i => i.TrendNumber == item.TrendNumber);

                

                this.OnAfterDeshboardCreated(item);

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
