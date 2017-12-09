using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public abstract class InventoryDataAccessBase
    {
        public InventoryDataAccessBase(Inventory inventory)
        {
            Inventory = inventory;
        }

        protected Inventory Inventory { get; }

        protected ThingDataContext GetContext()
        {
            return ThingDataContext.Create(Inventory);
        }
    }
}
