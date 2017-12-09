using System.Linq;
using System.Threading.Tasks;
using Thingventory.Core.Data;
using Thingventory.Core.Models;

namespace Thingventory.Core.Services
{
    public interface ILocationService
    {
        Task<Location> CreateLocationAsync(string name);
        Task<Location[]> GetLocationsAsync();
    }

    public sealed class LocationService : InventoryDataAccessBase, ILocationService, IService
    {
        public LocationService(Inventory inventory) : base(inventory)
        {
        }

        public async Task<Location> CreateLocationAsync(string name)
        {
            using (var context = GetContext())
            {
                var entity = new LocationEntity
                {
                    Name = name
                };

                context.Locations.Add(entity);
                await context.SaveChangesAsync();

                return _Translate(entity);
            }
        }

        public async Task<Location[]> GetLocationsAsync()
        {
            using (var context = GetContext())
            {
                return await context
                    .Locations
                    .ToAsyncEnumerable()
                    .Select(_Translate)
                    .ToArray();
            }
        }

        private Location _Translate(LocationEntity entity) => new Location(entity.Id) {Name = entity.Name};
    }
}
