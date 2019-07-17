using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.Api.Infrastructure;
using Catalog.Api.Model;
using Catalog.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : Controller
    {
        private readonly CatalogContext _catalogContext;

        public CatalogController(CatalogContext context)
        {
            this._catalogContext = context;
        }

        // GET api/v1/[controller]/items[?pageSize=3&pageIndex=10]
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0, string ids = null)
        {
            if (!string.IsNullOrEmpty(ids))
            {

                var items = await GetItemsByIdsAsync(ids);

                if (!items.Any())
                {
                    return BadRequest("ids value invalid. Must be comma-separated list of numbers");
                }

                return Ok(items);
            }

            //var totalItems = await _catalogContext.CatalogItems.LongCountAsync();

            //var itemsOnPage = await _catalogContext.CatalogItems
            //    .OrderBy(c => c.Name)
            //    .Skip(pageSize * pageIndex)
            //    .Take(pageSize)
            //    .ToListAsync();

            //var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            var totalItems = await _catalogContext.CatalogBrands.LongCountAsync();

            var itemsOnPage = await _catalogContext.CatalogBrands
                .OrderBy(c => c.Brand)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogBrand>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        private async Task<List<CatalogItem>> GetItemsByIdsAsync(string ids)
        {
            var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));

            if (!numIds.All(nid => nid.Ok))
            {
                return new List<CatalogItem>();
            }

            var idsToSelect = numIds
                .Select(id => id.Value);

            var items = await _catalogContext.CatalogItems.Where(ci => idsToSelect.Contains(ci.Id)).ToListAsync();

            return items;
        }

        // POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateProductAsync([FromBody]CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _catalogContext.CatalogItems.Add(item);

            await _catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<ActionResult<CatalogItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }
    }
}