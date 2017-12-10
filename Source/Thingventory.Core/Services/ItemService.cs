using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public interface IItemService
    {
        Task<ItemDetails> CreateItemAsync(ItemDetails item);
        Task<ItemDetails> GetItemDetailsAsync(int itemId);
        Task<ItemSummary[]> GetItemListForLocation(int locationId);
        Task SaveItemAsync(ItemDetails item);
    }

    public sealed class ItemService : InventoryDataAccessBase, IItemService, IService
    {
        public ItemService(Inventory inventory) : base(inventory)
        {
        }

        public async Task<ItemDetails> CreateItemAsync(ItemDetails item)
        {
            using (var context = GetContext())
            {
                var entity = _AddNewItem(context, item);

                _UpdateEntity(entity, item);

                await context.SaveChangesAsync();

                return _TranslateToDetails(entity);
            }
        }

        public async Task<ItemDetails> GetItemDetailsAsync(int itemId)
        {
            using (var context = GetContext())
            {
                var entity = await context
                    .Items
                    .FirstAsync(item => item.Id == itemId);

                return _TranslateToDetails(entity);
            }
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

        public async Task SaveItemAsync(ItemDetails item)
        {
            using (var context = GetContext())
            {
                var entity = await context.Items.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (entity == null)
                {
                    entity = _AddNewItem(context, item);
                }
                else
                {
                    item.UpdatedInstant = DateTimeOffset.Now;
                }

                _UpdateEntity(entity, item);

                await context.SaveChangesAsync();
                item.ResetHasChanges();
            }
        }

        private static ItemEntity _AddNewItem(ThingDataContext context, ItemDetails item)
        {
            var entity = new ItemEntity();
            context.Items.Add(entity);
            item.CreatedInstant = item.UpdatedInstant = DateTimeOffset.Now;

            return entity;
        }

        private ItemDetails _TranslateToDetails(ItemEntity item)
        {
            var details = new ItemDetails(item.Id)
            {
                AcquiredDate = item.AcquiredDate,
                AcquiredFrom = item.AcquiredFrom,
                Comments = item.Comments,
                CreatedInstant = item.CreatedInstant,
                LocationId = item.LocationId,
                Name = item.Name,
                Quantity = item.Quantity,
                UpdatedInstant = item.UpdatedInstant,
                Value = item.Value,
            };

            details.ResetHasChanges();

            return details;
        }

        private ItemSummary _TranslateToSummary(ItemEntity item) => new ItemSummary(item.Id, item.Name, item.LocationId);

        private void _UpdateEntity(ItemEntity entity, ItemDetails item)
        {
            entity.AcquiredDate = item.AcquiredDate;
            entity.AcquiredFrom = item.AcquiredFrom;
            entity.Comments = item.Comments;
            entity.CreatedInstant = item.CreatedInstant;
            entity.LocationId = item.LocationId;
            entity.Name = item.Name;
            entity.Quantity = item.Quantity;
            entity.UpdatedInstant = item.UpdatedInstant;
            entity.Value = item.Value;
        }
    }
}
