using System.Linq;
using System.Threading.Tasks;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public interface IItemService
    {
        Task<ItemSummary[]> GetItemListForLocation(int locationId);
    }

    public sealed class ItemService : InventoryDataAccessBase, IItemService, IService
    {
        public ItemService(Inventory inventory) : base(inventory)
        {
        }

        public async Task<ItemSummary[]> GetItemListForLocation(int locationId)
        {
            using (var context = GetContext())
            {
                var items = await context
                    .Items
                    .Where(item => item.LocationId == locationId)
                    .ToAsyncEnumerable()
                    .Select(_TranslateToSummary)
                    .ToArray();

                return items;
            }
        }

        private ItemSummary _TranslateToSummary(ItemEntity item) => new ItemSummary(item.Id, item.Name);
    }
}
